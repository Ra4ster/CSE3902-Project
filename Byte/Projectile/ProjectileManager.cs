using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Byte.Projectile;

// holds every projectile currently in the world and removes them once they expire
public class ProjectileManager
{
    private readonly List<IProjectile> projectiles = new List<IProjectile>();

    public void Add(IProjectile projectile) => projectiles.Add(projectile);

    public void Clear() => projectiles.Clear();

    public void Update(GameTime gameTime)
    {
        projectiles.RemoveAll(projectile => projectile.IsExpired || projectile.HasCollided);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (IProjectile projectile in projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}
