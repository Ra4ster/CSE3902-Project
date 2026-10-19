using Byte.Collidable;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Item
{
    public class Bomb : IItem, ICollidable
    {

        public bool IsCollected { get; set; } = false;
        public Vector2 Position { get; set; }
        private StaticSprite sprite;
        private float scale;


        public Bomb(StaticSprite sprite, Vector2 position, float scale = 5f)
        {
            this.sprite = sprite;
            Position = position;
            this.scale = scale;
        }

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
