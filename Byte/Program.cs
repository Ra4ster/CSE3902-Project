using Byte.Command;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Byte.Controller;
using Byte.Player;
using Byte.Projectile;
using Byte.Sprite;
using Byte;
using Byte.Rooms;
using Byte.Factory;
using Byte.Collidable;
using System.Collections.ObjectModel;


/// <summary>
/// Class representing the game, containing the graphics device and sprite painter. Also the entrypoint of the application.
/// </summary>
public class Program : Game
{
    public GraphicsDeviceManager graphicsDeviceManager;
    private const float SCREEN_FILL_RATIO = 0.85f;
    private const int LINK_SOURCE_RECT_COUNT = 10;

    public static bool DrawBoundingBoxes { get; set; } = true;
    public Link link = null!;
    public ISprite? linkSprite;
    private ProjectileManager projectileManager = new ProjectileManager();
    private RoomManager roomManager = null!;
    private readonly CollisionDispatcher dispatcher = new();
    private Door? pendingDoor;
    public static BoundingBoxDrawer BoundingBoxDrawer = null!;
    private SpriteBatch? SpritePainter { get; set; }
    private Matrix spriteTransform = Matrix.Identity;
    private SpriteFont statusFont = null!;

    readonly Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
    readonly Dictionary<Rectangle, ICommand> leftClickBindings = new Dictionary<Rectangle, ICommand>();
    readonly Dictionary<Rectangle, ICommand> rightClickBindings = new Dictionary<Rectangle, ICommand>();

    private readonly IController mouseControls;
    private readonly IController kbControls;

    private bool resetRequested = false;

    public Program()
    {
        kbControls = new KeyboardController(keybindings, [Keys.E, Keys.R, Keys.OemOpenBrackets, Keys.OemCloseBrackets]);
        mouseControls = new MouseController(leftClickBindings, rightClickBindings);

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        int displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int windowSide = Math.Min(GameConstants.WINDOW_SIZE.Height, (int)(displayHeight * SCREEN_FILL_RATIO));
        graphicsDeviceManager.PreferredBackBufferWidth = windowSide;
        graphicsDeviceManager.PreferredBackBufferHeight = windowSide;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;

        LinkCollisionHandler.Register(dispatcher);
        ProjectileCollisionHandler.Register(dispatcher);
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

    }

    protected override void LoadContent()
    {
        GameAssets.Load(Content);
        statusFont = Content.Load<SpriteFont>("Roboto");
        SpritePainter = new SpriteBatch(GraphicsDevice);
        BoundingBoxDrawer = new BoundingBoxDrawer(GraphicsDevice);
        BuildLevel();

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
                door.Properties.OnCollide = _ => pendingDoor = door;
    }

    // Link's position is his origin, not the top-left of his hitbox.
    private static Vector2 LinkPositionAt(Vector2 tileTopLeft) => tileTopLeft + Link.LinkOrigin * Link.SpriteScale;

    private void PlaceLink(Vector2 tileTopLeft)
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

        pendingDoor = null;
        Vector2 linkPos = LinkPositionAt(roomManager.CurrentRoom.PlayerSpawnPosition);
        link = new Link(linkPos, new StaticSprite(
            GameAssets.Instance.LinkSheet, linkPos,
            Color.White, new Rectangle(1, 11, 16, 16), 6.0f),
            SpritePainter!, GameAssets.Instance.LinkSheet,
            mouseControls, kbControls);

        link.sourceRects = new Collection<Rectangle>(
            Enumerable.Repeat(Rectangle.Empty, LINK_SOURCE_RECT_COUNT).ToList());

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
        rightClickBindings.Add(GameConstants.WINDOW_SIZE, quitCommand);
        keybindings.Add(Keys.Z, attackCommand);
        keybindings.Add(Keys.N, attackCommand);
        keybindings.Add(Keys.R, resetCommand);
        keybindings.Add(Keys.E, PlayerTakeDamageCommand);
        keybindings.Add(Keys.OemCloseBrackets, nextRoomCommand);
        keybindings.Add(Keys.OemOpenBrackets, previousRoomCommand);
        AddProjectileCommands();
    }

    private void AddProjectileCommands()
    {
        ProjectileFactory projectileFactory = new ProjectileFactory(GameAssets.Instance.LinkSheet, GameAssets.Instance.BossSheet);

        ICommand useArrowCommand = new UseProjectileCommand(link, projectileManager, projectileFactory.CreateArrow);
        ICommand useBombCommand = new UseProjectileCommand(link, projectileManager,
            (position, direction) => projectileFactory.CreateBomb(position, direction));
        ICommand useBoomerangCommand = new UseProjectileCommand(link, projectileManager, projectileFactory.CreateBoomerang);

        keybindings.Add(Keys.D1, useArrowCommand);
        keybindings.Add(Keys.D2, useBombCommand);
        keybindings.Add(Keys.D3, useBoomerangCommand);
    }

    protected override void Update(GameTime gameTime)
    {
        kbControls.Update(gameTime);
        mouseControls.Update(gameTime);

        if (resetRequested)
        {
            resetRequested = false;
            BuildLevel();
        }

        projectileManager.Update(gameTime);
        roomManager.Update(gameTime);

        link.Update(gameTime);
        ProcessCollisions();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(GameConstants.BG_COLOR);

        SpritePainter?.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.NonPremultiplied,
            samplerState: SamplerState.PointClamp, // Disables sampling outside pixel bounds
            transformMatrix: spriteTransform // Transforms drawing to a space separate from window bounds for resizing.
        );

        roomManager.Draw(SpritePainter!);
        link.state.Draw(link);
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
        if (link.Health <= 0)
            SpritePainter.DrawString(statusFont,
                "HP is zero. Collision testing stays active; press R to reset.",
                new Vector2(20, GameConstants.WINDOW_SIZE.Height - 100), Color.Black);

        SpritePainter?.End();

        base.Draw(gameTime);
    }
    private void ProcessCollisions()
    {
        IRoom room = roomManager.CurrentRoom;

        // Resolve barriers first, so body and sword contacts use the corrected position.
        foreach (ICollidable block in room.Blocks.OfType<ICollidable>())
            DispatchCollisions(new List<ICollidable> { link, block });
        LinkCollisionHandler.ClampToRoom(link, GameConstants.WINDOW_SIZE);

        foreach (ICollidable block in room.Blocks.OfType<ICollidable>())
        {
            foreach (IProjectile projectile in projectileManager.Projectiles)
            {
                DispatchCollisions(new List<ICollidable> { projectile, block });
            }
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
            PlaceLink(roomManager.EnterDoor(door));
        }
    }

    private void DispatchCollisions(IList<ICollidable> collidables)
    {
        var detector = new CollisionDetector();
        foreach (var collision in detector.DetectCollisions(collidables))
            dispatcher.Dispatch(collision.A, collision.B, collision.Side);
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