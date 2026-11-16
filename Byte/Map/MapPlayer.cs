
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Map
{
    internal sealed class MapPlayer
    {
        private readonly Texture2D pixel;

        public static Color color = Color.LimeGreen;

        public MapPlayer(Texture2D pixel) => this.pixel = pixel;

        public void Draw(SpriteBatch painter, Rectangle roomBounds, Vector2 offset)
        {
            int size = roomBounds.Height;
            Rectangle shiftedRect = new(
                roomBounds.Center.X - size / 2 + (int)offset.X,
                roomBounds.Center.Y - size / 2 + (int)offset.Y,
                size,
                size);
            painter.Draw(pixel, shiftedRect, color);
        }
    }
}