using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

/// <summary>
/// Moving, animating sprite
/// </summary>
public class MovingAnimatedSprite : ISprite
{
    protected Vector2 position { get; set; }
    protected Vector2 velocity { get; set; }

    private Texture2D texture;
    public Color Color { get; set; }

    private float scale;

    private Rectangle[] sourceRectangles;

    private int currentFrame;
    private float animationTimer = 0;
    private float frameDuration;

    public MovingAnimatedSprite(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
        Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
    {
        this.position = position;
        this.velocity = velocity;
        this.texture = texture;
        Color = color;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
        this.scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture,
            position,
            sourceRectangles[currentFrame],
            Color,
            0f,
            Vector2.Zero,
            scale,
            SpriteEffects.None,
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