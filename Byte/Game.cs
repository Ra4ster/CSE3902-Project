using Byte.Command;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint0.Controller;
using Sprint0.Sprite;
using Byte.Player;

/// <summary>
/// Class representing the game, containing the graphics device and sprite painter.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{

    public GraphicsDeviceManager graphicsDeviceManager;
    public static Rectangle WINDOW_SIZE = new Rectangle(0, 0, 1600, 1600);
    public static Rectangle[] quadrants = new Rectangle[4];

    public Link link;
    public ISprite linkSprite;

    private Texture2D? pixelTexture;

    public ISprite? ActiveSprite { get; set; }
    public ISprite? TextSprite { get; set; }

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

    protected override void LoadContent()
    {
        Texture2D bowser = Content.Load<Texture2D>("Bowser");
        SpriteFont roboto = Content.Load<SpriteFont>("Roboto");

        Texture2D linkSheet = Content.Load<Texture2D>("linkSheet4");

        SpritePainter = new SpriteBatch(GraphicsDevice);

        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData([Color.White]);

        Vector2 linkPos = new Vector2(220.0f);
        link = new Link(linkPos, new StaticSprite(linkSheet,linkPos,Color.White, new Rectangle(1, 11, 16, 16), 6.0f), SpritePainter,linkSheet,mouseControls, kbControls);

        Rectangle[] bowserFrames =
        {
            new Rectangle(17, 8, 42, 40),
            new Rectangle(65, 8, 42, 40),
            new Rectangle(113, 8, 42, 40),
            new Rectangle(161, 8, 42, 40),
            new Rectangle(209, 8, 42, 40)
        };

        int leftWidth = WINDOW_SIZE.Width / 2;
        int rightWidth = WINDOW_SIZE.Width - leftWidth;
        int topHeight = WINDOW_SIZE.Height / 2;
        int bottomHeight = WINDOW_SIZE.Height - topHeight;

        quadrants[0] = new Rectangle(WINDOW_SIZE.Left, WINDOW_SIZE.Top, leftWidth, topHeight);
        quadrants[1] = new Rectangle(WINDOW_SIZE.Left + leftWidth, WINDOW_SIZE.Top, rightWidth, topHeight);
        quadrants[2] = new Rectangle(WINDOW_SIZE.Left, WINDOW_SIZE.Top + topHeight, leftWidth, bottomHeight);
        quadrants[3] = new Rectangle(WINDOW_SIZE.Left + leftWidth, WINDOW_SIZE.Top + topHeight, rightWidth, bottomHeight);

        Vector2 animatedPos = new Vector2(500.0f);
        Vector2 staticPos = new Vector2(500.0f);
        Vector2 movingPos = new Vector2(500.0f);
        Vector2 fullPos = new Vector2(500.0f);

        link.sourceRects = new Rectangle[10];
        


        Vector2 textPos = new Vector2(50.0f, 1450.0f);

        Vector2 velocityX = new Vector2(-400.0f, 0.0f);
        Vector2 velocityY = new Vector2(0.0f, 800.0f);

        ICommand attackCommand = new AttackCommand(this, link);
        ICommand moveUpCommand = new MoveUpCommand(this, link);
        ICommand moveDownCommand = new MoveDownCommand(this, link);
        ICommand moveLeftCommand = new MoveLeftCommand(this, link);
        ICommand moveRightCommand = new MoveRightCommand(this, link);
        ICommand quitCommand = new QuitCommand(this);
        ICommand staticSpriteCommand = new SetStaticSpriteCommand(this, bowser, ref staticPos, Color.White, ref bowserFrames[0], 4.0f);
        ICommand animatedSpriteCommand = new SetAnimatedSpriteCommand(this, bowser, ref animatedPos, Color.White, bowserFrames, 0.1f, 4.0f);
        ICommand movingSpriteCommand = new SetMovingSpriteCommand(this, bowser, ref movingPos, Color.White, ref velocityY, ref bowserFrames[0], 4.0f);
        ICommand movingAnimatedSpriteCommand = new MovingAnimatedSpriteCommand(this, bowser, ref fullPos, ref velocityX, Color.White, bowserFrames, 0.1f, 4.0f);

        keybindings.Add(Keys.D0, quitCommand);
        keybindings.Add(Keys.W, moveUpCommand);
        keybindings.Add(Keys.S, moveDownCommand);
        keybindings.Add(Keys.A, moveLeftCommand);
        keybindings.Add(Keys.D, moveRightCommand);
        rightClickBindings.Add(WINDOW_SIZE, quitCommand);
        keybindings.Add(Keys.D1, staticSpriteCommand);
        leftClickBindings.Add(quadrants[0], staticSpriteCommand);
        keybindings.Add(Keys.D2, animatedSpriteCommand);
        leftClickBindings.Add(quadrants[1], animatedSpriteCommand);
        keybindings.Add(Keys.D3, movingSpriteCommand);
        leftClickBindings.Add(quadrants[2], movingSpriteCommand);
        keybindings.Add(Keys.D4, movingAnimatedSpriteCommand);
        leftClickBindings.Add(quadrants[3], movingAnimatedSpriteCommand);
        keybindings.Add(Keys.I, attackCommand);

        base.LoadContent();
        staticSpriteCommand.Execute(new GameTime());
    }

    protected override void Update(GameTime gameTime)
    {
        kbControls.Update(gameTime);
        mouseControls.Update(gameTime);
        ActiveSprite?.Update(gameTime);
        TextSprite?.Update(gameTime);
        link.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        SpritePainter?.Begin();

        // Quadrants
        SpritePainter?.Draw(pixelTexture, quadrants[0], Color.LightPink);
        SpritePainter?.Draw(pixelTexture, quadrants[1], Color.LightGreen);
        SpritePainter?.Draw(pixelTexture, quadrants[2], Color.LightSkyBlue);
        SpritePainter?.Draw(pixelTexture, quadrants[3], Color.LightGoldenrodYellow);
        link.state.Draw(link);
        // Sprites
        ActiveSprite?.Draw(SpritePainter!);
        TextSprite?.Draw(SpritePainter!);

        SpritePainter?.End();

        base.Draw(gameTime);
    }

    public static void Main() => new Game().Run();
}