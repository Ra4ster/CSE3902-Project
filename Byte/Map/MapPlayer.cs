
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
            painter.Draw(pixel, rect, color);
        }
    }
}