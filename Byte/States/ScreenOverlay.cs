using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.States;

public sealed class ScreenOverlay : IDisposable
{
    private const float Half = 0.5f;

    private readonly Texture2D pixel;
    private readonly SpriteFont font;

    public ScreenOverlay(GraphicsDevice graphicsDevice, SpriteFont font)
    {
        this.font = font;
        pixel = new Texture2D(graphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    public void Fill(SpriteBatch spriteBatch, Rectangle area, Color color)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        spriteBatch.Draw(pixel, area, color);
    }

    public void FillScreen(SpriteBatch spriteBatch, Color color) => Fill(spriteBatch, GameConstants.WINDOW_SIZE, color);

    public float DrawCenteredText(SpriteBatch spriteBatch, string text, float centerY, Color color, float scale)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        Vector2 size = font.MeasureString(text) * scale;
        Vector2 position = new((GameConstants.WINDOW_SIZE.Width - size.X) * Half, centerY - size.Y * Half);
        spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        return position.X;
    }

    public void DrawTextAt(SpriteBatch spriteBatch, string text, float left, float centerY, Color color, float scale)
    {
        ArgumentNullException.ThrowIfNull(spriteBatch);
        float height = font.MeasureString(text).Y * scale;
        spriteBatch.DrawString(font, text, new Vector2(left, centerY - height * Half), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    public void Dispose() => pixel.Dispose();
}
