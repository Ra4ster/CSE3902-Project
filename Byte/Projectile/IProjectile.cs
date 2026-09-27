using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// something that is fired or thrown, moves on its own, and goes away when it's done
public interface IProjectile
{
    // true once the projectile is finished and should be removed
    bool IsExpired { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);
}
