using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite;


/// <summary>
/// Non-moving, animated sprite
/// </summary>
public class AnimatedSprite : ISprite
{
    
    public bool Loop { get; set; } = true;
    public bool IsFinished { get; set; } = false;
    private Vector2[]? frameOrigins;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    private readonly Texture2D texture;
    private readonly Vector2 position;
    public Color Color { get; set; } = Color.White;
    public Vector2 Origin { get; set; } = Vector2.Zero;

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

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 frameOrigin = Origin;
        if (frameOrigins != null)
        {
            frameOrigin = frameOrigins[currentFrame];
        }

        spriteBatch.Draw(texture, position, sourceRectangles[currentFrame], Color, 0f, frameOrigin, scale, Effects, 0f);
    }
    public void Update(GameTime gameTime)
    {
        animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (animationTimer >= frameDuration)
        {
            currentFrame = (currentFrame + 1) % sourceRectangles.Length; // loop around
            animationTimer = 0;
        }
    }

    public void MoveOrigin(Vector2 origin)
    {

        Origin = origin;
    }
    public void SetFrameOrigin(Vector2[] Origins)
    {
        frameOrigins = Origins;
    }
}