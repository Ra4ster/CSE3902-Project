using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite;

/// <summary>
/// Non-moving, animated sprite
/// </summary>
public class AnimatedSprite : ISprite
{
    private readonly Texture2D texture;
    private readonly Vector2 position;
    public Color Color { get; set; }

    private Rectangle[] sourceRectangles;
    private float scale;
    private readonly double frameDuration;

    private int currentFrame;
    private float animationTimer = 0;

    public AnimatedSprite(Texture2D texture, ref Vector2 position, Color color, Rectangle[] sourceRectangles, float frameDuration, float scale = 1f)
    {
        this.position = position;
        this.texture = texture;
        Color = color;
        this.scale = scale;
        this.sourceRectangles = sourceRectangles;
        this.frameDuration = frameDuration;
    }

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, sourceRectangles[currentFrame], Color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

    public void Update(GameTime gameTime)
    {
        animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (animationTimer >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % sourceRectangles.Length; // loop around
            animationTimer = 0;
        }
    }
}