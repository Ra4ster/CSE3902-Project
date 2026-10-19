using Byte.Collidable;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    internal class Bow : IItem, ICollidable
    {
        public Vector2 Position { get; set; }
        private readonly StaticSprite sprite;
        public bool IsCollected { get; set; } = false;

        private float scale;
        public Rectangle BoundingBox => new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(8 * scale),
            (int)(16 * scale)
        );
        public CollisionProperties Properties { get; } = new CollisionProperties
        {
            Type = CollidableTypes.ItemPickup,
            IsStatic = true
        };

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
