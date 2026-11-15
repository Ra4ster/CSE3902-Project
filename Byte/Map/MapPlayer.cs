
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Map
{
    internal sealed class MapPlayer : Drawing.IDrawable
    {
        private readonly Rectangle rect;
        private readonly Texture2D pixel;

        public static Color color = Color.LimeGreen;

        public MapPlayer(Rectangle placement, Texture2D pixel)
        {
            rect = placement;
            this.pixel = pixel;
        }

        public void Draw(SpriteBatch painter)
        {
            Draw(painter, Vector2.Zero);
        }

        public void Draw(SpriteBatch painter, Vector2 offset)
        {
            Rectangle shiftedRect = new(
                rect.X + (int)offset.X,
                rect.Y + (int)offset.Y,
                rect.Width,
                rect.Height);
            painter.Draw(pixel, shiftedRect, color);
        }
    }
}