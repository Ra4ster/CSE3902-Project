using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

/// <summary>
/// Moving, animating sprite
/// </summary>
public class MovingAnimatedSprite : ISprite
{
    protected Vector2 position;
    protected Vector2 velocity;

    protected Texture2D texture;
    protected Color color { get; set; }

    protected float scale;

    protected SpriteEffects spriteEffects;

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
        this.color = color;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
        this.scale = scale;
        this.spriteEffects = spriteEffects;
    }

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
            color,
            0f,
            Vector2.Zero,
            scale,
            spriteEffects,
            0f);
    }

    public virtual void Update(GameTime gameTime)
    {
        float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Moving
        position += velocity * seconds;

        // Animating
        animationTimer += seconds;

        if (animationTimer >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % sourceRectangles.Length;
            animationTimer = 0;
        }
    }

    public void SetPos(int x, int y)
    {
        position = new Vector2(x, y);
    }
}