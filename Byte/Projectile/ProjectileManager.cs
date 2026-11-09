using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile;

// holds every projectile currently in the world and removes them once they expire
public class ProjectileManager
{
    private readonly List<IProjectile> projectiles = new List<IProjectile>();

    public List<IProjectile> Projectiles => projectiles;

    public void Add(IProjectile projectile) => projectiles.Add(projectile);

    public void Clear() => projectiles.Clear();

    public void Update(GameTime gameTime)
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            IProjectile projectile = projectiles[i];

            projectile.Update(gameTime);

            if (projectile.IsExpired || projectile.HasCollided)
            {
                if (projectile is Boomerang)
                {
                    Program.soundManager.StopBoomerangSound(projectile);
                }

                projectiles.RemoveAt(i);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (IProjectile projectile in projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}
