
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Collidable
{
    public class BoundingBoxDrawer
    {
        private readonly Texture2D pixel;

        public BoundingBoxDrawer(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle bounds, Color color)
        {
            int thickness = 2;

            // Top
            spriteBatch.Draw(pixel,
                new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness),
                color);

            // Bottom
            spriteBatch.Draw(pixel,
                new Rectangle(bounds.X, bounds.Bottom - thickness,
                    bounds.Width, thickness),
                color);

            // Left
            spriteBatch.Draw(pixel,
                new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height),
                color);

            // Right
            spriteBatch.Draw(pixel,
                new Rectangle(bounds.Right - thickness, bounds.Y,
                    thickness, bounds.Height),
                color);
        }
    }
}
