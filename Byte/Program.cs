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

/// <summary>
/// Class representing the game, containing the graphics device and sprite painter. Also the entrypoint of the application.
/// </summary>
public class Program : Game
{
    public GraphicsDeviceManager graphicsDeviceManager;
    private const float SCREEN_FILL_RATIO = 0.85f;

    public static bool DrawBoundingBoxes { get; set; } = true;
    public Link link = null!;
    public ISprite? linkSprite;
    private ProjectileManager projectileManager = new ProjectileManager();
    private RoomManager roomManager = null!;
    private readonly CollisionDispatcher dispatcher = new();
    public static BoundingBoxDrawer BoundingBoxDrawer = null!;
    private SpriteBatch? SpritePainter { get; set; }
    private Matrix spriteTransform = Matrix.Identity;
    private SpriteFont statusFont = null!;

    Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
    Dictionary<Rectangle, ICommand> leftClickBindings = new Dictionary<Rectangle, ICommand>();
    Dictionary<Rectangle, ICommand> rightClickBindings = new Dictionary<Rectangle, ICommand>();

    private IController mouseControls;
    private IController kbControls;

    private bool resetRequested = false;

    public Program()
    {
        kbControls = new KeyboardController(keybindings, [Keys.E, Keys.R]);
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
    }

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

        Vector2 linkPos = roomManager.CurrentRoom.PlayerSpawnPosition;
        link = new Link(linkPos, new StaticSprite(
            GameAssets.Instance.LinkSheet, linkPos,
            Color.White, new Rectangle(1, 11, 16, 16), 6.0f),
            SpritePainter!, GameAssets.Instance.LinkSheet,
            mouseControls, kbControls);

        link.sourceRects = new Rectangle[10];

        ICommand attackCommand = new AttackCommand(this, link);
        ICommand moveUpCommand = new MoveUpCommand(this, link);
        ICommand moveDownCommand = new MoveDownCommand(this, link);
        ICommand moveLeftCommand = new MoveLeftCommand(this, link);
        ICommand moveRightCommand = new MoveRightCommand(this, link);
        ICommand quitCommand = new QuitCommand(this);
        ICommand resetCommand = new ResetCommand(this);
        ICommand PlayerTakeDamageCommand = new PlayerTakeDamageCommand(link);
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
        AddProjectileCommands();
    }

    private void AddProjectileCommands()
    {
        ProjectileFactory projectileFactory = new ProjectileFactory(GameAssets.Instance.LinkSheet, GameAssets.Instance.BossSheet);

        ICommand useArrowCommand = new UseProjectileCommand(link, projectileManager, projectileFactory.CreateArrow);
        ICommand useBombCommand = new UseProjectileCommand(link, projectileManager,
            (position, direction) => projectileFactory.CreateBomb(position));
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

        link.state.Draw(link);
        roomManager.Draw(SpritePainter!);
        projectileManager.Draw(SpritePainter!);
        if (DrawBoundingBoxes)
        {
            BoundingBoxDrawer.Draw(SpritePainter!, link.BoundingBox, Color.Red);
            if (!link.Sword.BoundingBox.IsEmpty)
                BoundingBoxDrawer.Draw(SpritePainter!, link.Sword.BoundingBox, Color.Yellow);

            foreach (ICollidable collidable in GetRoomCollidables())
                BoundingBoxDrawer.Draw(SpritePainter!, collidable.BoundingBox, Color.Green);
        }

        SpritePainter!.DrawString(statusFont,
            $"HP: {link.Health:0}   Hits taken: {link.HitsTaken}   R: Reset",
            new Vector2(20, 20), Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        if (link.Health <= 0)
            SpritePainter.DrawString(statusFont,
                "HP is zero. Collision testing stays active; press R to reset.",
                new Vector2(20, 75), Color.Black);

        SpritePainter?.End();

        base.Draw(gameTime);
    }
    private void ProcessCollisions()
    {
        foreach (ICollidable block in roomManager.CurrentRoom.Blocks.OfType<ICollidable>())
            DispatchCollisions(new List<ICollidable> { link, block });
        LinkCollisionHandler.ClampToRoom(link, GameConstants.WINDOW_SIZE);

        var collidables = new List<ICollidable> { link };
        collidables.AddRange(roomManager.CurrentRoom.Enemies);

        link.Sword.Update(link);
        if (!link.Sword.BoundingBox.IsEmpty)
            collidables.Add(link.Sword);
        DispatchCollisions(collidables);
    }

    private void DispatchCollisions(IList<ICollidable> collidables)
    {
        var detector = new CollisionDetector();
        foreach (var collision in detector.DetectCollisions(collidables))
            dispatcher.Dispatch(collision.A, collision.B, collision.Side);
    }
    private IEnumerable<ICollidable> GetRoomCollidables() =>
        roomManager.CurrentRoom.Blocks.OfType<ICollidable>()
            .Concat(roomManager.CurrentRoom.Enemies);

    public static void Main() => new Program().Run();
}