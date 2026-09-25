using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite;

/// <summary>
/// Non-moving, non-animating sprite.
/// </summary>
public class StaticSprite : ISprite
{
    public bool Loop { get; set; } = true;
    public bool IsFinished { get;  set; } = false;
    private readonly Texture2D texture;
    private readonly Vector2 position;
    public Color Color { get; set; }
    private float scale;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    private readonly Rectangle sourceRectangle;

    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    public StaticSprite(Texture2D texture, Vector2 position, Color color, Rectangle sourceRectangle, float scale = 1f)
    {
        this.texture = texture;
        this.position = position;
        Color = color;
        this.scale = scale;
        this.sourceRectangle = sourceRectangle;
    }

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, sourceRectangle, Color, 0f, Origin, scale, Effects, 0f);

    public void Update(GameTime gameTime)
    {
        // Does nothing! It is not moving or animating.
    }
    public void MoveOrigin(Vector2 origin)
    {

        Origin = origin;
    }
    public void SetFrameOrigin(Vector2[] Origins)
    {
        //does nothing
    }
}