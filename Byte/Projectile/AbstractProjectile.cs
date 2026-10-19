using Byte.Collidable;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Projectile
{
    public class AbstractProjectile : IProjectile
    {
        private readonly ISprite sprite;

        private readonly Point size;

        public Vector2 Position { get; set; }

        bool HasCollided { get; set;  }

        bool IProjectile.HasCollided => HasCollided;

        bool IsExpired { get; }

        bool IProjectile.IsExpired => IsExpired;

        Rectangle ICollidable.BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, size.X, size.Y);

        public CollisionProperties Properties { get; } = new CollisionProperties
        {
            Type = CollidableTypes.Projectile,
            IsStatic = false,
            DamagesEnemies = true,
    };
        void IProjectile.Collided() => HasCollided = true;

        public void Update(GameTime gameTime) => sprite.Update(gameTime);

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!HasCollided || IsExpired)
                sprite.Draw(spriteBatch);
        }
    }
}
