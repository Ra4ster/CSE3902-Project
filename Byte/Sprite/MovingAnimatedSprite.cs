
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint0.Sprite;

/// <summary>
/// Moving, animating sprite
/// </summary>
public class MovingAnimatedSprite : ISprite
{
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    private Vector2 position;
    private Vector2 velocity;
    private Texture2D texture;
    public Color Color { get; set; }

    private float scale;

    private Rectangle[] sourceRectangles;

    private int currentFrame;
    private float animationTimer = 0;
    private float frameDuration;

    public MovingAnimatedSprite(Texture2D texture, Vector2 position,  Vector2 velocity, Color color,
    Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
    {
        this.texture = texture;
        this.position = position;
        this.velocity = velocity;
        Color = color;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
        this.scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, sourceRectangles[currentFrame], Color, 0f, Vector2.Zero, scale, Effects, 0f);
    }

    public void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // 1. Moving
        /*
        if (position.X < Game.WINDOW_SIZE.Left || position.X > Game.WINDOW_SIZE.Right - (sourceRectangles[currentFrame].Width * scale))
            velocity = new Vector2(velocity.X * -1.0f, velocity.Y);

        position.X += velocity.X * seconds;

        if (position.Y > Game.WINDOW_SIZE.Bottom - (sourceRectangles[currentFrame].Height * scale) || position.Y < Game.WINDOW_SIZE.Top)
            velocity = new Vector2(velocity.X, velocity.Y * -1.0f);

        position.Y += velocity.Y * seconds;
        */

        // 2. Animating
        animationTimer += seconds;
        if (animationTimer >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % sourceRectangles.Length;
            animationTimer = 0;
        }
    }

    public void SetPos(Vector2 position)
    {
        //position.X = x;
        //position.Y = y;
        this.position = position;
    }
}