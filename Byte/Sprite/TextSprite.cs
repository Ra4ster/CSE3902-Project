using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

/// <summary>
/// Sprite containing string to be rendered with font.
/// </summary>
public class TextSprite : ISprite
{
    public bool Loop { get; set; } = true;
    public bool IsFinished { get;  set; } = false;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    private readonly SpriteFont font;
    public string Text { get; set; }
    private readonly Vector2 position;
    public Color Color { get; set; } = Color.White;
    private float scale;


    public TextSprite(SpriteFont font, string text, ref Vector2 position, Color color, float scale = 1f)
    {
        this.font = font;
        Text = text;
        this.position = position;
        this.Color = color;
        this.scale = scale;
    }

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.DrawString(font, Text, position, Color, 0f, Origin, scale, SpriteEffects.None, 0f);

    public void Update(GameTime gameTime) {/* This doesn't animate */}

    public void MoveOrigin(Vector2 origin)
    {

       // Does nothing in this iSprite  
    }
    public void SetFrameOrigin(Vector2[] Origins)
    {
        // does nothing
    }
}