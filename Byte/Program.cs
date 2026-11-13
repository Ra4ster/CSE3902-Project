using Byte;
using Byte.Collidable;
using Byte.Command;
using Byte.Controller;
using Byte.Core;
using Byte.Drawing;
using Byte.Factory;
using Byte.InventoryAndHud;
using Byte.Player;
using Byte.Projectile;
using Byte.Rooms;
using Byte.Sprite;
using Byte.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.ObjectModel;
using Byte.Sounds;


/// <summary>
/// Class representing the game, containing the graphics device and sprite painter. Also the entrypoint of the application.
/// </summary>
public class Program : Game
{
    public GraphicsDeviceManager graphicsDeviceManager;
    private const float SCREEN_FILL_RATIO = 0.85f;
    private const int LINK_SOURCE_RECT_COUNT = 10;
    private const int HALF_HEARTS_PER_HEART = 2;

    public static bool DrawBoundingBoxes { get; set; } = true;
    public Link link = null!;
    public ISprite? linkSprite;
    private ProjectileManager projectileManager = new ProjectileManager();
    private RoomManager roomManager = null!;
    private readonly CollisionDispatcher dispatcher = new();
    private Door? pendingDoor;
    public static BoundingBoxDrawer BoundingBoxDrawer = null!;
    private SpriteBatch? SpritePainter { get; set; }
    private DrawSystem drawSystem = null!;
    private Matrix spriteTransform = Matrix.Identity;
    private SpriteFont statusFont = null!;

    public GameStateMachine StateMachine { get; } = new();
    private ScreenOverlay overlay = null!;

    readonly Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
    readonly Dictionary<Rectangle, ICommand> leftClickBindings = new Dictionary<Rectangle, ICommand>();
    readonly Dictionary<Rectangle, ICommand> rightClickBindings = new Dictionary<Rectangle, ICommand>();

    private readonly IController mouseControls;
    private readonly IController kbControls;

    public static SoundManager soundManager = new SoundManager();

    private bool resetRequested = false;

    public InventoryData inventoryData = new();
    public InventoryManager? inventoryManager;

    public SelectionController selectionController = new(new InventoryData());
    private InventoryAndHUDGraphics inventoryGraphics;

    public Program()
    {
        kbControls = new KeyboardController(keybindings, [Keys.E, Keys.Enter, Keys.R, Keys.H, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.I, Keys.P, Keys.T]);
        mouseControls = new MouseController(leftClickBindings, rightClickBindings);

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        int displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int windowHeight = Math.Min(GameConstants.WINDOW_SIZE.Height, (int)(displayHeight * SCREEN_FILL_RATIO));
        graphicsDeviceManager.PreferredBackBufferWidth = windowHeight * GameConstants.WINDOW_SIZE.Width / GameConstants.WINDOW_SIZE.Height;
        graphicsDeviceManager.PreferredBackBufferHeight = windowHeight;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;

        LinkCollisionHandler.Register(dispatcher);
        ProjectileCollisionHandler.Register(dispatcher);
        EnemyCollisionHandler.Register(dispatcher);

        GameEvents.TriforcePickedUp += OnTriforcePickedUp;
        LinkCollisionHandler.PickUpItemEvent += GameEvents.RaiseItemPickedUp;
    }

    private void OnTriforcePickedUp()
    {
        if (StateMachine.Current is GameplayState)
            StateMachine.ChangeState(new WinState(this));
    }

    private void OnClientSizeChanged(object? sender, EventArgs e)
    {
        Viewport viewport = GraphicsDevice.Viewport;
        float scale = Math.Min(
            viewport.Width / (float)GameConstants.WINDOW_SIZE.Width,
            viewport.Height / (float)GameConstants.WINDOW_SIZE.Height);
        float offsetX = (viewport.Width - GameConstants.WINDOW_SIZE.Width * scale) / 2.0f;
        float offsetY = (viewport.Height - GameConstants.WINDOW_SIZE.Height * scale) / 2.0f;

        spriteTransform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(offsetX, offsetY, 0.0f);
        drawSystem?.SetSpriteTransform(spriteTransform);

    }

    protected override void LoadContent()
    {
        soundManager.LoadSound(Content);
        GameAssets.Load(Content);
        statusFont = Content.Load<SpriteFont>("Roboto");
        SpritePainter = new SpriteBatch(GraphicsDevice);
        drawSystem = new DrawSystem(GraphicsDevice, SpritePainter);
        BoundingBoxDrawer = new BoundingBoxDrawer(GraphicsDevice);
        overlay = new ScreenOverlay(GraphicsDevice, statusFont);
        BuildLevel();
        StateMachine.Start(new GameplayState(this));

        soundManager.PlayMusic();

        OnClientSizeChanged(this, EventArgs.Empty);
        base.LoadContent();
    }

    /// <summary>
    /// Asks for a full level reload; it runs after input is handled so the keybindings aren't rebuilt mid-loop.
    /// </summary>
    public void RequestReset() => resetRequested = true;

    private void LoadRooms()
    {
        string roomsDirectory = Path.Combine(AppContext.BaseDirectory, "Content", "Rooms");
        RoomData[] rooms = Directory.GetFiles(roomsDirectory, "*.json") // Get all .json files in Content/Rooms and convert to DTO
            .Select(path => RoomLoader.GetRoomData(Path.Combine("Content", "Rooms", Path.GetFileName(path))))
            .ToArray();

        EnemyFactory enemyFactory = new EnemyFactory(projectileManager);
        BlockFactory blockFactory = new BlockFactory(GameAssets.Instance.BlockSheet);
        ItemFactory itemFactory = new ItemFactory();
        roomManager = new RoomManager(rooms, enemyFactory, blockFactory, itemFactory);

        foreach (IRoom room in roomManager.Rooms)
            foreach (Door door in room.Doors)
                door.Properties.OnCollide = _ =>
                {
                    if (door.CanEnter)
                        pendingDoor = door;
                };
    }

    // Link's position is his origin, not the top-left of his hitbox.
    internal static Vector2 LinkPositionAt(Vector2 tileTopLeft) => tileTopLeft + Link.LinkOrigin * Link.SpriteScale;

    internal void PlaceLink(Vector2 tileTopLeft)
    {
        link.CorrectPosition(LinkPositionAt(tileTopLeft));
        link.Velocity = Vector2.Zero;
        projectileManager.Clear();
    }

    public void NextRoom() => PlaceLink(roomManager.NextRoom());

    public void PreviousRoom() => PlaceLink(roomManager.PreviousRoom());

    /// <summary>
    /// Builds every object in the level from the roomManager, along with the commands bound to them.
    /// </summary>
    private void BuildLevel()
    {
        keybindings.Clear();
        leftClickBindings.Clear();
        rightClickBindings.Clear();

        projectileManager = new ProjectileManager();
        LoadRooms();

        inventoryManager?.Dispose();
        inventoryData = new InventoryData();
        inventoryManager = new InventoryManager(inventoryData);

        pendingDoor = null;
        Vector2 linkPos = LinkPositionAt(roomManager.CurrentRoom.PlayerSpawnPosition);
        link = new Link(linkPos, new StaticSprite(
            GameAssets.Instance.LinkSheet, linkPos,
            Color.White, new Rectangle(1, 11, 16, 16), 6.0f),
            SpritePainter!, GameAssets.Instance.LinkSheet,
            mouseControls, kbControls);

        link.sourceRects = new Collection<Rectangle>(
            Enumerable.Repeat(Rectangle.Empty, LINK_SOURCE_RECT_COUNT).ToList());

        selectionController = new SelectionController(inventoryData);
        inventoryGraphics = new InventoryAndHUDGraphics(
            GameAssets.Instance.ItemSheet,
            GameAssets.Instance.InventorySheet,
            statusFont,
            inventoryData,
            selectionController
        );
        ICommand openInventoryCommand = new ChangeStateCommand(StateMachine, () => new InventoryState(this));
        ICommand pauseCommand = new ChangeStateCommand(StateMachine, () => new PausedState(this));
        ICommand triggerWinCommand = new TriggerWinCommand();
        ICommand attackCommand = new AttackCommand(this, link);
        ICommand moveUpCommand = new MoveUpCommand(this, link);
        ICommand moveDownCommand = new MoveDownCommand(this, link);
        ICommand moveLeftCommand = new MoveLeftCommand(this, link);
        ICommand moveRightCommand = new MoveRightCommand(this, link);
        ICommand quitCommand = new QuitCommand(this);
        ICommand resetCommand = new ResetCommand(this);
        ICommand PlayerTakeDamageCommand = new PlayerTakeDamageCommand(link);
        ICommand nextRoomCommand = new NextRoomCommand(this);
        ICommand previousRoomCommand = new PreviousRoomCommand(this);
        ICommand toggleBoundingBoxesCommand = new ToggleBoundingBoxesCommand();
        ICommand pauseMusic  = new PauseMusicCommand();
        ICommand interactWithDoorCommand = new InteractWithDoorCommand(this);
        keybindings.Add(Keys.D0, quitCommand);
        keybindings.Add(Keys.Q, quitCommand);
        keybindings.Add(Keys.W, moveUpCommand);
        keybindings.Add(Keys.S, moveDownCommand);
        keybindings.Add(Keys.A, moveLeftCommand);
        keybindings.Add(Keys.D, moveRightCommand);
        keybindings.Add(Keys.Up, moveUpCommand);
        keybindings.Add(Keys.Down, moveDownCommand);
        keybindings.Add(Keys.Left, moveLeftCommand);
        keybindings.Add(Keys.Right, moveRightCommand);
        leftClickBindings.Add(GameConstants.WINDOW_SIZE, previousRoomCommand);
        rightClickBindings.Add(GameConstants.WINDOW_SIZE, nextRoomCommand);
        keybindings.Add(Keys.Z, attackCommand);
        keybindings.Add(Keys.N, attackCommand);
        keybindings.Add(Keys.R, resetCommand);
        keybindings.Add(Keys.E, PlayerTakeDamageCommand);
        keybindings.Add(Keys.Enter, interactWithDoorCommand);
        keybindings.Add(Keys.OemCloseBrackets, nextRoomCommand);
        keybindings.Add(Keys.OemOpenBrackets, previousRoomCommand);
        keybindings.Add(Keys.H, toggleBoundingBoxesCommand);
        keybindings.Add(Keys.M, pauseMusic);
        keybindings.Add(Keys.I, openInventoryCommand);
        keybindings.Add(Keys.P, pauseCommand);
        keybindings.Add(Keys.T, triggerWinCommand);

        AddProjectileCommands();
        
    }

    private void AddProjectileCommands()
    {
        ProjectileFactory projectileFactory = new ProjectileFactory(
                GameAssets.Instance.LinkSheet,
                GameAssets.Instance.BossSheet
            );

        Func<IProjectile, bool> canUse = projectile => inventoryManager!.CanUseItem(projectile);

        ICommand useArrowCommand = new UseProjectileCommand(link, projectileManager, projectileFactory.CreateArrow, canUse);
        ICommand useBombCommand = new UseProjectileCommand(link, projectileManager,
            (position, _) => projectileFactory.CreateBomb(position), canUse);
        ICommand useBoomerangCommand = new UseProjectileCommand(link, projectileManager, projectileFactory.CreateBoomerang, canUse);

        keybindings.Add(Keys.D1, useArrowCommand);
        keybindings.Add(Keys.D2, useBombCommand);
        keybindings.Add(Keys.D3, useBoomerangCommand);
        
    }

    protected override void Update(GameTime gameTime)
    {
        

        StateMachine.Update(gameTime);

        if (resetRequested)
        {
            resetRequested = false;
            BuildLevel();
            StateMachine.Start(new GameplayState(this));
        }

        base.Update(gameTime);
    }

    internal void UpdateGameplayInput(GameTime gameTime)
    {
        kbControls.Update(gameTime);
        mouseControls.Update(gameTime);
    }

    internal void InteractWithDoor()
    {
        IRoom room = roomManager.CurrentRoom;
        Door? door = room.Doors.FirstOrDefault(candidate =>
            !candidate.CanEnter &&
            candidate.InteractionBounds(room.Bounds).Intersects(link.BoundingBox));
        door?.TryOpen();
    }

    internal void UpdateWorld(GameTime gameTime)
    {
        projectileManager.Update(gameTime);
        roomManager.Update(gameTime);
        link.Update(gameTime);
        ProcessCollisions();
        soundManager.Update();
        base.Update(gameTime);
        inventoryData.CurrentHalfHearts = (int)(link.Health * HALF_HEARTS_PER_HEART);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(GameConstants.BG_COLOR);
        StateMachine.Draw(SpritePainter!);
        base.Draw(gameTime);
    }

    internal RoomManager Rooms => roomManager;
    internal SelectionController Selection => selectionController;
    internal ScreenOverlay Overlay => overlay;
    internal SpriteBatch Painter => SpritePainter!;

    internal void DrawScene(Vector2 cameraOffset)
    {
        BeginWorld(RoomOffset(roomManager.CurrentRoom) + cameraOffset);

        drawSystem.DrawRoom(roomManager.CurrentRoom);
        drawSystem.DrawLink(link);
        projectileManager.Draw(SpritePainter!);
        if (DrawBoundingBoxes)
        {
            BoundingBoxDrawer.Draw(SpritePainter!, link.BoundingBox, Color.Red);
            if (!link.Sword.BoundingBox.IsEmpty)
                BoundingBoxDrawer.Draw(SpritePainter!, link.Sword.BoundingBox, Color.Yellow);

            foreach (ICollidable collidable in roomManager.CurrentRoom.Collidables)
                BoundingBoxDrawer.Draw(SpritePainter!, collidable.BoundingBox, BoundingBoxColor(collidable));

            foreach (ICollidable projectile in projectileManager.Projectiles)
            {
                BoundingBoxDrawer.Draw(SpritePainter!, projectile.BoundingBox, Color.Cyan);
            }
        }

        SpritePainter!.DrawString(statusFont,
            $"{roomManager.CurrentRoom.Name}   HP: {link.Health:0}   Hits taken: {link.HitsTaken}",
            new Vector2(20, GameConstants.WINDOW_SIZE.Height - 60), Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);

        EndBatch();
    }

    internal void DrawLink(Vector2 cameraOffset)
    {
        drawSystem.DrawLink(link, link.Position + RoomOffset(roomManager.CurrentRoom) + cameraOffset);
    }

    internal void DrawLinkAt(Vector2 worldPosition, Rectangle clip) =>
        drawSystem.DrawLink(link, worldPosition, clip);

    internal void DrawRoom(IRoom room, Vector2 shift, Rectangle clip)
    {
        BeginWorld(RoomOffset(room) + shift, clip);
        drawSystem.DrawRoom(room);
        EndBatch();
    }

    internal void DrawHud(float slideOffset)
    {
        BeginScreen();
        inventoryGraphics.Draw(SpritePainter!, slideOffset);
        EndBatch();
    }

    internal void BeginWorld(Vector2 translation, Rectangle? clip = null)
        => drawSystem.Begin(translation, clip);

    internal void BeginScreen() => BeginWorld(Vector2.Zero);

    internal void EndBatch() => drawSystem.EndBatch();

    internal Rectangle RoomViewport(IRoom room)
    {
        Vector2 offset = RoomOffset(room);
        int frameMargin = 2 * (int)RoomGrid.TILE_WORLD_SIZE;
        return new Rectangle(
            (int)offset.X - frameMargin,
            (int)offset.Y - frameMargin,
            room.Bounds.Width + 2 * frameMargin,
            room.Bounds.Height + 2 * frameMargin);
    }

    private void ProcessCollisions()
    {
        IRoom room = roomManager.CurrentRoom;

        // Resolve barriers first, so body and sword contacts use the corrected position.
        foreach (ICollidable block in room.Blocks.OfType<ICollidable>())
            DispatchCollisions(new List<ICollidable> { link, block });
        Vector2 roomOffset = RoomOffset(room);
        Rectangle visibleWorldBounds = new(
            (int)-roomOffset.X,
            (int)-roomOffset.Y,
            GameConstants.WINDOW_SIZE.Width,
            GameConstants.WINDOW_SIZE.Height);
        LinkCollisionHandler.ClampToRoom(link, visibleWorldBounds);
        LinkCollisionHandler.ClampToRoom(link, room.Bounds, room.Doors);

        foreach (ICollidable block in room.Blocks.OfType<ICollidable>())
        {
            foreach (IProjectile projectile in projectileManager.Projectiles)
                DispatchCollisions(new List<ICollidable> { projectile, block });
            foreach (ICollidable enemy in room.Enemies)
                DispatchCollisions(new List<ICollidable> { enemy, block });
        }

        var collidables = new List<ICollidable> { link };
        collidables.AddRange(room.Enemies);
        collidables.AddRange(room.Items);
        collidables.AddRange(room.Doors);
        collidables.AddRange(projectileManager.Projectiles);

        link.Sword.Update(link);
        if (!link.Sword.BoundingBox.IsEmpty)
            collidables.Add(link.Sword);
        DispatchCollisions(collidables);

        // Switch rooms after dispatch so handlers don't see a half-swapped room.
        if (pendingDoor is Door door)
        {
            pendingDoor = null;
            StateMachine.ChangeState(new RoomTransitionState(this, door));
        }
    }

    private void DispatchCollisions(IList<ICollidable> collidables)
    {
        var detector = new CollisionDetector();
        foreach (var collision in detector.DetectCollisions(collidables))
            dispatcher.Dispatch(collision.A, collision.B, collision.Side);
    }

    internal static Vector2 RoomOffset(IRoom room)
    {
        Rectangle bounds = room.Bounds;
        float playHeight = GameConstants.WINDOW_SIZE.Height - InventoryAndHUDGraphics.HudHeight;
        return new Vector2(
            (GameConstants.WINDOW_SIZE.Width - bounds.Width) / 2f,
            InventoryAndHUDGraphics.HudHeight + Math.Max(0f, (playHeight - bounds.Height) / 2f)
        );
    }

    private static Color BoundingBoxColor(ICollidable collidable)
    {
        string type = collidable.Properties.Type;
        if (type == CollidableTypes.Door)
            return Color.Blue;
        if (type == CollidableTypes.ItemPickup)
            return Color.Magenta;
        if (type == CollidableTypes.Enemy)
            return Color.Orange;
        if (!collidable.Properties.BlocksPlayer)
            return Color.LightGreen;
        return Color.Green;
    }

    public static void Main() => new Program().Run();
}