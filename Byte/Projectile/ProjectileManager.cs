using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Byte.Projectile;

// holds every projectile currently in the world and removes them once they expire
public class ProjectileManager
{
    private readonly List<IProjectile> projectiles = new List<IProjectile>();

    public void Add(IProjectile projectile) => projectiles.Add(projectile);

    public void Update(GameTime gameTime)
    {
        foreach (IProjectile projectile in projectiles)
        {
            projectile.Update(gameTime);
        }
        projectiles.RemoveAll(projectile => projectile.IsExpired);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (IProjectile projectile in projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}
