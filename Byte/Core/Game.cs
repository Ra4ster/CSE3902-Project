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

/// <summary>
/// Class representing the game, containing the graphics device and sprite painter.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{

    public GraphicsDeviceManager graphicsDeviceManager;
    public static Rectangle WINDOW_SIZE = new Rectangle(0, 0, 1600, 1600);
    public static Color BG_COLOR = Color.SkyBlue;
    private const float SCREEN_FILL_RATIO = 0.85f;

    public Link link = null!;
    public ISprite? linkSprite;

    private Texture2D? bgTexture;

    private readonly IDungeon dungeon;

    private EnemyManager enemyManager = new EnemyManager();
    private BlockManager blockManager = new BlockManager();
    private ProjectileManager projectileManager = new ProjectileManager();
    private ItemManager itemManager = new ItemManager();

    private SpriteBatch? SpritePainter { get; set; }

    Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
    Dictionary<Rectangle, ICommand> leftClickBindings = new Dictionary<Rectangle, ICommand>();
    Dictionary<Rectangle, ICommand> rightClickBindings = new Dictionary<Rectangle, ICommand>();

    private IController mouseControls;
    private IController kbControls;

    public Game(IDungeon dungeon)
    {
        this.dungeon = dungeon;

        kbControls = new KeyboardController(keybindings);
        mouseControls = new MouseController(leftClickBindings, rightClickBindings);

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

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

    protected override void LoadContent()
    {
        GameAssets.Load(Content);
        enemyManager = dungeon.CreateEnemies();
        itemManager = dungeon.CreateItems();
        blockManager = dungeon.CreateBlocks();

        SpritePainter = new SpriteBatch(GraphicsDevice);

        bgTexture = new Texture2D(GraphicsDevice, 1, 1);
        bgTexture.SetData([Color.White]);

        Vector2 linkPos = new Vector2(220.0f);
        link = new Link(linkPos, new StaticSprite(
            GameAssets.Instance.LinkSheet, linkPos,
            Color.White, new Rectangle(1, 11, 16, 16), 6.0f),
            SpritePainter, GameAssets.Instance.LinkSheet,
            mouseControls, kbControls);

        ICommand NextItemCommand = new NextItemCommand(itemManager);
        ICommand PreviousItemCommand = new PreviousItemCommand(itemManager);
        keybindings.Add(Keys.I, NextItemCommand);
        keybindings.Add(Keys.U, PreviousItemCommand);

        link.sourceRects = new Rectangle[10];

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
        keybindings.Add(Keys.W, moveUpCommand);
        keybindings.Add(Keys.S, moveDownCommand);
        keybindings.Add(Keys.A, moveLeftCommand);
        keybindings.Add(Keys.D, moveRightCommand);
        rightClickBindings.Add(WINDOW_SIZE, quitCommand);
        keybindings.Add(Keys.Z, attackCommand);
        keybindings.Add(Keys.N, attackCommand);
        keybindings.Add(Keys.Q, resetCommand);
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

        enemyManager.Update(gameTime);
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
            samplerState: SamplerState.PointClamp
        );

        SpritePainter?.Draw(bgTexture, WINDOW_SIZE, BG_COLOR);
        link.state.Draw(link);

        enemyManager.Draw(SpritePainter!);
        blockManager.Draw(SpritePainter!);
        projectileManager.Draw(SpritePainter!);
        itemManager.Draw(SpritePainter!);

        SpritePainter?.End();

        base.Draw(gameTime);
    }

    public static void Main() => new Game(new Dungeon1()).Run();
}