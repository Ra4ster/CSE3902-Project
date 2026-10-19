using Byte.Collidable;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile
{
    public abstract class AbstractProjectile : IProjectile
    {
        protected readonly ISprite sprite;
        protected readonly Point size;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; } 

        public bool HasCollided { get; protected set; }
        bool IProjectile.HasCollided => HasCollided;

        public bool IsExpired { get; protected set; }
        bool IProjectile.IsExpired => IsExpired;

        Rectangle ICollidable.BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, size.X, size.Y);

        public CollisionProperties Properties { get; } = new CollisionProperties
        {
            Type = CollidableTypes.Projectile,
            IsStatic = false,
            DamagesEnemies = true,
        };

        protected AbstractProjectile(ISprite sprite, Point size, Vector2 position, Vector2 velocity)
        {
            this.sprite = sprite;
            this.size = size;
            Position = position;
            Velocity = velocity;
        }

        void IProjectile.Collided() => HasCollided = true;

        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!HasCollided && !IsExpired)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}