using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

/// <summary>
/// Sprite containing string to be rendered with font.
/// </summary>
public class TextSprite : ISprite
{
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    private readonly SpriteFont font;
    public string Text { get; set; }
    private readonly Vector2 position;
    private readonly Color color;
    private float scale;

    public TextSprite(SpriteFont font, string text, ref Vector2 position, Color color, float scale = 1f)
    {
        this.font = font;
        Text = text;
        this.position = position;
        this.color = color;
        this.scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.DrawString(font, Text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

    public void Update(GameTime gameTime) {/* This doesn't animate */}

}