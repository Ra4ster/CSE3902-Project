using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

/// <summary>
/// Moving, animating sprite
/// </summary>
public class MovingAnimatedSprite : ISprite
{
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    protected Vector2 position;
    protected Vector2 velocity;
    protected Texture2D texture;
    public Color Color { get; set; }
    protected Color color { get => Color; set => Color = value; }

    protected float scale;
    protected Rectangle[] sourceRectangles;

    protected int currentFrame;
    protected float animationTimer = 0;
    protected float frameDuration;

    public MovingAnimatedSprite(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
        Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f, SpriteEffects spriteEffects = SpriteEffects.None)
    {
        this.position = position;
        this.velocity = velocity;
        this.texture = texture;
        Color = color;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
        this.scale = scale;
        Effects = spriteEffects;
    }

    public MovingAnimatedSprite(Texture2D texture, Vector2 position, Vector2 velocity, Color color,
        Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
        : this(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        Vector2 drawPosition = new Vector2(
            MathF.Round(position.X),
            MathF.Round(position.Y)
        );

        spriteBatch.Draw(
            texture,
            drawPosition,
            sourceRectangles[currentFrame],
                Color,
            0f,
            Vector2.Zero,
            scale,
                Effects,
            0f);
    }

    public virtual void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        position += velocity * seconds;

        animationTimer += seconds;

        if (animationTimer >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % sourceRectangles.Length;
            animationTimer = 0;
        }
    }

    public void SetPos(Vector2 position)
    {
        this.position = position;
    }
}