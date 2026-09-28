using Byte.Block;
using Byte.Command;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Byte.Controller;
using Byte.Item;
using Byte.Player;
using Byte.Projectile;
using Byte.Sprite;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Class representing the game, containing the graphics device and sprite painter.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{

    public GraphicsDeviceManager graphicsDeviceManager;
    public static Rectangle WINDOW_SIZE = new Rectangle(0, 0, 1600, 1600);
    public static Rectangle[] quadrants = new Rectangle[4];
    private const float SCREEN_FILL_RATIO = 0.85f;

    public Link link = null!;
    public ISprite? linkSprite;

    private Texture2D? pixelTexture;

    private List<AbstractEnemy> enemies = new List<AbstractEnemy>();

    private BlockManager blockManager = new BlockManager();

    private ProjectileManager projectileManager = new ProjectileManager();

    private SpriteBatch? SpritePainter { get; set; }

    Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
    Dictionary<Rectangle, ICommand> leftClickBindings = new Dictionary<Rectangle, ICommand>();
    Dictionary<Rectangle, ICommand> rightClickBindings = new Dictionary<Rectangle, ICommand>();

    private IController mouseControls;
    private IController kbControls;

    ItemManager itemManager = new ItemManager();

    public Game()
    {
        kbControls = new KeyboardController(keybindings);
        mouseControls = new MouseController(leftClickBindings, rightClickBindings);

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // WINDOW_SIZE is the virtual resolution; the real window may be smaller and is scaled in Draw.
        int displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        int windowSide = Math.Min(WINDOW_SIZE.Height, (int)(displayHeight * SCREEN_FILL_RATIO));
        graphicsDeviceManager.PreferredBackBufferWidth = windowSide;
        graphicsDeviceManager.PreferredBackBufferHeight = windowSide;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;
    }

    private void OnClientSizeChanged(object? sender, EventArgs e)
    {
        graphicsDeviceManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
        graphicsDeviceManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
        graphicsDeviceManager.ApplyChanges();
    }

    /// <summary>
    /// Scales the virtual WINDOW_SIZE to fit the current window, centered with letterboxing.
    /// </summary>
    private Matrix GetScreenTransform()
    {
        Viewport viewport = GraphicsDevice.Viewport;
        float scale = Math.Min(
            viewport.Width / (float)WINDOW_SIZE.Width,
            viewport.Height / (float)WINDOW_SIZE.Height);
        float offsetX = (viewport.Width - WINDOW_SIZE.Width * scale) / 2f;
        float offsetY = (viewport.Height - WINDOW_SIZE.Height * scale) / 2f;
        return Matrix.CreateScale(scale) * Matrix.CreateTranslation(offsetX, offsetY, 0f);
    }

    private void AddEnemies()
    {
        enemies.Add(EnemyFactory.Instance.CreateStalfos(
        [
            new Vector2(100, 100),
            new Vector2(100, 300),
            new Vector2(300, 300),
            new Vector2(300, 100)
        ], 200f));

        enemies.Add(EnemyFactory.Instance.CreateKeese(
        [
            new Vector2(500, 600),
            new Vector2(350, 450),
            new Vector2(300, 400),
            new Vector2(250, 450)
        ], 200f));

        enemies.Add(EnemyFactory.Instance.CreateGel(
        [
            new Vector2(300, 800),
            new Vector2(800, 800)
        ], 200f, 1.5f));

        enemies.Add(EnemyFactory.Instance.CreateAquamentus(
        [
            new Vector2(800, 900),
            new Vector2(1000, 900)
        ], 200f, 1.5f));
    }

    private void AddBlocks()
    {
        BlockFactory blockFactory = new BlockFactory(GameAssets.Instance.BlockSheet);
        Vector2 blockPos = new Vector2(1200, 300);

        blockManager.AddBlock(blockFactory.CreateSquareBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateKeystoneBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateStatueLeftBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateStatueRightBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateVoidBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateSpeckledFloorBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateWaterBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateStairsBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateBrickBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateLadderBlock(blockPos));
        blockManager.AddBlock(blockFactory.CreateSandBlock(blockPos));
    }

    protected override void LoadContent()
    {
        GameAssets.Load(Content); // TODO: Wrap item sheet in GameAssets
        SpritePainter = new SpriteBatch(GraphicsDevice);

        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData([Color.White]);

        Vector2 linkPos = new Vector2(220.0f);
        link = new Link(linkPos, new StaticSprite(
            GameAssets.Instance.LinkSheet, linkPos,
            Color.White, new Rectangle(1, 11, 16, 16), 6.0f),
            SpritePainter, GameAssets.Instance.LinkSheet,
            mouseControls, kbControls);

        int leftWidth = WINDOW_SIZE.Width / 2;
        int rightWidth = WINDOW_SIZE.Width - leftWidth;
        int topHeight = WINDOW_SIZE.Height / 2;
        int bottomHeight = WINDOW_SIZE.Height - topHeight;

        //Code dealing with items
        ItemFactory itemFactory = new ItemFactory();


        Vector2 spawnPos = new Vector2(700,700);

        itemManager.AddItem(itemFactory.CreateRupee(spawnPos));
        itemManager.AddItem(itemFactory.CreateHeart(spawnPos));
        itemManager.AddItem(itemFactory.CreateBoomerang(spawnPos));
        itemManager.AddItem(itemFactory.CreateBow(spawnPos));
        itemManager.AddItem(itemFactory.CreateBomb(spawnPos));

        ICommand NextItemCommand = new NextItemCommand(itemManager);
        ICommand PreviousItemCommand = new PreviousItemCommand(itemManager);
        keybindings.Add(Keys.I, NextItemCommand);
        keybindings.Add(Keys.U, PreviousItemCommand);


        quadrants[0] = new Rectangle(WINDOW_SIZE.Left, WINDOW_SIZE.Top, leftWidth, topHeight);
        quadrants[1] = new Rectangle(WINDOW_SIZE.Left + leftWidth, WINDOW_SIZE.Top, rightWidth, topHeight);
        quadrants[2] = new Rectangle(WINDOW_SIZE.Left, WINDOW_SIZE.Top + topHeight, leftWidth, bottomHeight);
        quadrants[3] = new Rectangle(WINDOW_SIZE.Left + leftWidth, WINDOW_SIZE.Top + topHeight, rightWidth, bottomHeight);

        link.sourceRects = new Rectangle[10];
        AddEnemies();
        AddBlocks();


        ICommand attackCommand = new AttackCommand(this, link);
        ICommand moveUpCommand = new MoveUpCommand(this, link);
        ICommand moveDownCommand = new MoveDownCommand(this, link);
        ICommand moveLeftCommand = new MoveLeftCommand(this, link);
        ICommand moveRightCommand = new MoveRightCommand(this, link);
        ICommand quitCommand = new QuitCommand(this);
        ICommand resetCommand = new ResetCommand(this, link, blockManager, projectileManager);
        ICommand nextBlockCommand = new NextBlockCommand(blockManager);
        ICommand previousBlockCommand = new PreviousBlockCommand(blockManager);

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
        rightClickBindings.Add(WINDOW_SIZE, quitCommand);
        keybindings.Add(Keys.Z, attackCommand);
        keybindings.Add(Keys.N, attackCommand);
        keybindings.Add(Keys.R, resetCommand);
        keybindings.Add(Keys.T, previousBlockCommand);
        keybindings.Add(Keys.Y, nextBlockCommand);

        AddProjectileCommands();

        base.LoadContent();
    }

    private void AddProjectileCommands()
    {
        ProjectileFactory projectileFactory = new ProjectileFactory(GameAssets.Instance.LinkSheet);

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

        foreach (AbstractEnemy enemy in enemies) enemy.Update(gameTime);

        blockManager.Update(gameTime);
        projectileManager.Update(gameTime);
        itemManager.Update(gameTime);
        link.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        SpritePainter?.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.NonPremultiplied,
            samplerState: SamplerState.PointClamp,
            transformMatrix: GetScreenTransform()
    );

        // Quadrants
        SpritePainter?.Draw(pixelTexture, quadrants[0], Color.LightPink);
        SpritePainter?.Draw(pixelTexture, quadrants[1], Color.LightGreen);
        SpritePainter?.Draw(pixelTexture, quadrants[2], Color.LightSkyBlue);
        SpritePainter?.Draw(pixelTexture, quadrants[3], Color.LightGoldenrodYellow);
        link.state.Draw(link);

        foreach (AbstractEnemy enemy in enemies)
            enemy.Draw(SpritePainter!);

        blockManager.Draw(SpritePainter!);
        projectileManager.Draw(SpritePainter!);

        itemManager.Draw(SpritePainter!);

        SpritePainter?.End();

        base.Draw(gameTime);
    }

    public static void Main() => new Game().Run();
}