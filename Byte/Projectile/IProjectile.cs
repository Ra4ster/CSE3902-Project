using Byte.Collidable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// something that is fired or thrown, moves on its own, and goes away when it's done
public interface IProjectile : ICollidable 
{
    // true once the projectile is finished and should be removed
    bool IsExpired { get; }
    
    // True when projectile hits something and should disappear
    bool HasCollided { get; }
    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);

    void Collided();
    new CollisionProperties Properties { get; set; }
}

