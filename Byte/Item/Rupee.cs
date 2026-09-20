using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    internal class Rupee
    {
        public Vector2 Position { get; set; }

        private Texture2D spriteSheet;
        private List<Rectangle> sourceRectangles;
        private int currentFrame;
        private double timer;
        private double frameInterval = 5000;
        private int spriteSheetPosX;
        private int spriteSheetPosY;
        private Rectangle[] sourceRectangles;
        private int frameWidth;
        private int frameHeight;
        public Rupee(Texture2D texture, Vector2 position)
        {
            spriteSheet = texture;
            Position = position;
            currentFrame = 0;
            timer = 0;

            sourceRectangles = new List<Rectangle>
            {
                new Rectangle(0, 0, spriteSheetPosX, spriteSheetPosY),
                new Rectangle(8, 0, 8, 16)
            };
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer >= frameInterval)
            {
                currentFrame = (currentFrame + 1) % sourceRectangles.Count;
                timer = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (spriteSheet != null)
            {
                Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRectangles[currentFrame].Width * 2), (int)(sourceRectangles[currentFrame].Height * 2));

                spriteBatch.Draw(
                    spriteSheet,
                    destinationRectangle,
                    sourceRectangles[currentFrame],
                    Color.White
                );
            }
        }
    }
}
