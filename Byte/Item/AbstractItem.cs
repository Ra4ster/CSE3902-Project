using Byte.Collidable;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Item
{
    // shared logic for every pickup - a sprite at a fixed spot that goes away when link touches it
    public abstract class AbstractItem : IItem
    {
        private readonly ISprite sprite;
        private readonly Point size;

        public Vector2 Position { get; set; }
        public bool IsCollected { get; private set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, size.X, size.Y);
        public CollisionProperties Properties { get; } = new CollisionProperties
        {
            Type = CollidableTypes.ItemPickup,
            IsStatic = true
        };

        protected AbstractItem(ISprite sprite, Vector2 position, Point size)
        {
            this.sprite = sprite;
            this.size = size;
            Position = position;
        }

        public void Collect() => IsCollected = true;

        public void Update(GameTime gameTime) => sprite.Update(gameTime);

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
                sprite.Draw(spriteBatch);
        }
    }
}
