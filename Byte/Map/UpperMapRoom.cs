using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Map
{
    internal sealed class UpperMapRoom : Drawing.IDrawable
    {
        private readonly Rectangle destRect;
        private readonly Rectangle sourceRect;
        private readonly Texture2D spriteSheet;

        internal Rectangle Bounds => destRect;

        public UpperMapRoom(Rectangle placement, Rectangle sourceRect, Texture2D spriteSheet)
        {
            destRect = placement;
            this.sourceRect = sourceRect;
            this.spriteSheet = spriteSheet;
        }

        public void Draw(SpriteBatch painter)
        {
            Draw(painter, Vector2.Zero);
        }

        public void Draw(SpriteBatch painter, Vector2 offset)
        {
            Rectangle shiftedRect = new(
                destRect.X + (int)offset.X,
                destRect.Y + (int)offset.Y,
                destRect.Width,
                destRect.Height);

            painter.Draw(spriteSheet, shiftedRect, sourceRect, Color.White);
        }
    }
}