using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    internal class Rupee
    {
        private readonly Texture2D texture;
        private readonly Vector2 position;
        public Color Color { get; set; }

        private Rectangle[] sourceRectangles;
        private float scale;
        private readonly double frameDuration;

        private int currentFrame;
        private float animationTimer = 0;

        public Rupee(Texture2D texture, ref Vector2 position, Color color, Rectangle[] sourceRectangles, float frameDuration, float scale = 1f)
        {
            this.position = position;
            this.texture = texture;
            Color = color;
            this.scale = scale;
            this.sourceRectangles = sourceRectangles;
            this.frameDuration = frameDuration;
        }

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, position, sourceRectangles[currentFrame], Color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        public void Update(GameTime gameTime)
        {
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer >= frameDuration)
            {
                currentFrame = (currentFrame + 1) % sourceRectangles.Length; 
                animationTimer = 0;
            }
        }
}
