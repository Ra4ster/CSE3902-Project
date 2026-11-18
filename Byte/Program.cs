using Byte;
using Byte.Collidable;
using Byte.Command;
using Byte.Controller;
using Byte.Drawing;
using Byte.Factory;
using Byte.InventoryAndHud;
using Byte.Map;
using Byte.Player;
using Byte.Projectile;
using Byte.Rooms;
using Byte.Sounds;
using Byte.Sprite;
using Byte.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;


/// <summary>
/// Class representing the game, containing the graphics device and sprite painter. Also the entrypoint of the application.
/// </summary>
public class Program : Game
{
    public GraphicsDeviceManager graphicsDeviceManager;
    private const float SCREEN_FILL_RATIO = 0.85f;
    private const int LINK_SOURCE_RECT_COUNT = 10;
    private const int HALF_HEARTS_PER_HEART = 2;

    public static bool DrawBoundingBoxes { get; set; } = false;
    public Link link = null!;
    public ISprite? linkSprite;
    private RoomManager roomManager = null!;
    private readonly CollisionDispatcher dispatcher = new();
    private CollisionSystem collisionSystem = null!;
    public static BoundingBoxDrawer BoundingBoxDrawer = null!;
    private SpriteBatch? SpritePainter { get; set; }
    private DrawSystem drawSystem = null!;
    private Matrix spriteTransform = Matrix.Identity;
    private SpriteFont statusFont = null!;

    public GameStateMachine StateMachine { get; } = new();
    private ScreenOverlay overlay = null!;

    private readonly InputControllers inputControllers;

    public static SoundManager soundManager = SoundManager.Instance;

    private bool resetRequested = false;

    public InventoryData inventoryData = new();
    public InventoryManager? inventoryManager;

    public SelectionController selectionController = new(new InventoryData());
    private InventoryAndHUDGraphics? inventoryGraphics;

    public UpperMap? upperMap;

    public Program()
    {
        inputControllers = new InputControllers(CommandConstants.PressOnlyKeys);

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

    public void NextRoom()
    {
        PlaceLink(roomManager.NextRoom());
        upperMap?.OnRoomEntered(roomManager.CurrentRoom.Name);

    }

    public void PreviousRoom()
    {
        PlaceLink(roomManager.PreviousRoom());
        upperMap?.OnRoomEntered(roomManager.CurrentRoom.Name);
    }

    protected override void LoadContent()
    {
        soundManager.LoadSound(Content);
        GameAssets.Load(Content);
        statusFont = Content.Load<SpriteFont>("NES");
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

        EnemyFactory enemyFactory = new EnemyFactory();
        BlockFactory blockFactory = new BlockFactory(GameAssets.Instance.BlockSheet);
        ItemFactory itemFactory = new ItemFactory();
        roomManager = new RoomManager(rooms, enemyFactory, blockFactory, itemFactory);

    }

    // Link's position is his origin, not the top-left of his hitbox.
    internal static Vector2 LinkPositionAt(Vector2 tileTopLeft) => tileTopLeft + Link.LinkOrigin * Link.SpriteScale;

    internal void PlaceLink(Vector2 tileTopLeft)
    {
        link.CorrectPosition(LinkPositionAt(tileTopLeft));
        link.Velocity = Vector2.Zero;
        ProjectileManager.Instance.Clear();
        upperMap?.OnRoomEntered(roomManager.CurrentRoom.Name);

    }

    /// <summary>
    /// Builds every object in the level from the roomManager, along with the commands bound to them.
    /// </summary>
    private void BuildLevel()
    {
        LoadRooms();
        LevelRepresentation levelRep = new LevelRepresentation(roomManager);

        upperMap = new UpperMap(
        levelRep,
        GameAssets.Instance.InventorySheet,
        new Rectangle(
        (int)(4f * GameConstants.SCALE),
        (int)(4f * GameConstants.SCALE),
        (int)(32f * GameConstants.SCALE),
        (int)(32f * GameConstants.SCALE)
        )
        );

        inventoryManager?.Dispose();
        inventoryData = new InventoryData();
        inventoryManager = new InventoryManager(inventoryData, selectionController);

        Vector2 linkPos = LinkPositionAt(roomManager.CurrentRoom.PlayerSpawnPosition);
        link = new Link(linkPos, new StaticSprite(
            GameAssets.Instance.LinkSheet, linkPos,
            Color.White, new Rectangle(1, 11, 16, 16), 6.0f),
            SpritePainter!, GameAssets.Instance.LinkSheet,
            inputControllers.Mouse, inputControllers.Keyboard);

        link.sourceRects = new Collection<Rectangle>(
            Enumerable.Repeat(Rectangle.Empty, LINK_SOURCE_RECT_COUNT).ToList());

        collisionSystem = new CollisionSystem(roomManager.Rooms, link, dispatcher);

        selectionController = new SelectionController(inventoryData);
        inventoryGraphics = new InventoryAndHUDGraphics(
            GameAssets.Instance.ItemSheet,
            GameAssets.Instance.InventorySheet,
            statusFont,
            inventoryData,
            selectionController,
            new Byte.Map.LevelRepresentation(roomManager),
            upperMap
        );
        CommandConstants.Configure(
            inputControllers,
            this,
            link,
            inventoryManager!,
            inventoryData,
            selectionController
        );
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
        inputControllers.Update(gameTime);
    }

    internal void InteractWithDoor()
    {
        IRoom room = roomManager.CurrentRoom;
        Door? door = room.Doors.FirstOrDefault(candidate =>
            !candidate.CanEnter &&
            candidate.InteractionBounds(room.Bounds).Intersects(link.BoundingBox));

        if (door == null) return;

        if (door.State == Byte.Core.Rooms.DoorState.Locked && inventoryData.Keys > 0)
        {
            inventoryData.Keys--;
            _ = door.TryUnlock();
            soundManager.PlaySound("Door_Unlock");
        }
        else
            _ = door.TryOpen();
    }

    internal void UpdateWorld(GameTime gameTime)
    {
        ProjectileManager.Instance.Update(gameTime);
        roomManager.Update(gameTime);
        link.Update(gameTime);
        Door? door = collisionSystem.ProcessCollisions(roomManager.CurrentRoom);
        if (door is not null)
            StateMachine.ChangeState(new RoomTransitionState(this, door));
        selectionController.Update();
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
        BeginWorld(RoomLayout.RoomOffset(roomManager.CurrentRoom) + cameraOffset);

        drawSystem.DrawRoom(roomManager.CurrentRoom);
        drawSystem.DrawLink(link);
        ProjectileManager.Instance.Draw(SpritePainter!);
        if (DrawBoundingBoxes)
        {
            BoundingBoxDrawer.Draw(SpritePainter!, link.BoundingBox, Color.Red);
            if (!link.Sword.BoundingBox.IsEmpty)
                BoundingBoxDrawer.Draw(SpritePainter!, link.Sword.BoundingBox, Color.Yellow);

            foreach (ICollidable collidable in roomManager.CurrentRoom.Collidables)
                BoundingBoxDrawer.Draw(SpritePainter!, collidable.BoundingBox, BoundingBoxColor(collidable));

            foreach (ICollidable projectile in ProjectileManager.Instance.Projectiles)
                BoundingBoxDrawer.Draw(SpritePainter!, projectile.BoundingBox, Color.Cyan);
        }

        const float stringScale = 1.5f;
        const int startY = 60;
        SpritePainter!.DrawString(statusFont,
            $"{roomManager.CurrentRoom.Name}   HP: {link.Health:0}   Hits taken: {link.HitsTaken}",
            new Vector2(20, GameConstants.WINDOW_SIZE.Height - startY), Color.Black, 0f, Vector2.Zero, stringScale, SpriteEffects.None, 0f);

        EndBatch();
    }

    internal void DrawLink(Vector2 cameraOffset)
    {
        drawSystem.DrawLink(link, link.Position + RoomLayout.RoomOffset(roomManager.CurrentRoom) + cameraOffset);
    }

    internal void DrawLinkAt(Vector2 worldPosition, Rectangle clip) =>
        drawSystem.DrawLink(link, worldPosition, clip);

    internal void DrawRoom(IRoom room, Vector2 shift, Rectangle clip)
    {
        BeginWorld(RoomLayout.RoomOffset(room) + shift, clip);
        drawSystem.DrawRoom(room);
        EndBatch();
    }

    internal void DrawHud(float slideOffset)
    {
        BeginScreen();
        inventoryGraphics!.Draw(SpritePainter!, slideOffset, roomManager.CurrentRoom.Name);
        EndBatch();
    }

    internal void BeginWorld(Vector2 translation, Rectangle? clip = null)
        => drawSystem.Begin(translation, clip);

    internal void BeginScreen() => BeginWorld(Vector2.Zero);

    internal void EndBatch() => drawSystem.EndBatch();

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

    public static void Main()
    {
        using var game = new Program();
        game.Run();
    }
}