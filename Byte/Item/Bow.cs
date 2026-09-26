using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    internal class Bow : IItem
    {
        public Vector2 Position { get; set; }
        private readonly StaticSprite sprite;

        public Bow(StaticSprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
