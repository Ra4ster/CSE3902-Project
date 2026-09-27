using Byte.Command;
using Byte.Controller;
using Byte.Item;
using Byte.Player;
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

    public Link link = null!;
    public ISprite? linkSprite;

    private Texture2D? pixelTexture;

    private List<AbstractEnemy> enemies = new List<AbstractEnemy>();

    private SpriteBatch? SpritePainter { get; set; }

    Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
    Dictionary<Rectangle, ICommand> leftClickBindings = new Dictionary<Rectangle, ICommand>();
    Dictionary<Rectangle, ICommand> rightClickBindings = new Dictionary<Rectangle, ICommand>();

    private IController mouseControls;
    private IController kbControls;

    public Game()
    {
        kbControls = new KeyboardController(keybindings);
        mouseControls = new MouseController(leftClickBindings, rightClickBindings);

        graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        graphicsDeviceManager.PreferredBackBufferWidth = WINDOW_SIZE.Width;
        graphicsDeviceManager.PreferredBackBufferHeight = WINDOW_SIZE.Height;
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

    protected override void LoadContent()
    {
        Texture2D itemSheet = Content.Load<Texture2D>("items");

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

        //Code dealing with items:
        Texture2D itemsTexture = Content.Load<Texture2D>("items");

        ItemFactory itemFactory = new ItemFactory(itemsTexture);
        ItemManager itemManager = new ItemManager();

        Vector2 spawnPos = new Vector2(500,500);

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


        ICommand attackCommand = new AttackCommand(this, link);
        ICommand moveUpCommand = new MoveUpCommand(this, link);
        ICommand moveDownCommand = new MoveDownCommand(this, link);
        ICommand moveLeftCommand = new MoveLeftCommand(this, link);
        ICommand moveRightCommand = new MoveRightCommand(this, link);
        ICommand quitCommand = new QuitCommand(this);
        ICommand resetCommand = new ResetCommand(this, link);

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

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        kbControls.Update(gameTime);
        mouseControls.Update(gameTime);

        foreach (AbstractEnemy enemy in enemies) enemy.Update(gameTime);

        link.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        SpritePainter?.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.NonPremultiplied,
            samplerState: SamplerState.PointClamp
    );

        // Quadrants
        SpritePainter?.Draw(pixelTexture, quadrants[0], Color.LightPink);
        SpritePainter?.Draw(pixelTexture, quadrants[1], Color.LightGreen);
        SpritePainter?.Draw(pixelTexture, quadrants[2], Color.LightSkyBlue);
        SpritePainter?.Draw(pixelTexture, quadrants[3], Color.LightGoldenrodYellow);
        link.state.Draw(link);

        foreach (AbstractEnemy enemy in enemies)
            enemy.Draw(SpritePainter!);

        SpritePainter?.End();

        base.Draw(gameTime);
    }

    public static void Main() => new Game().Run();
}