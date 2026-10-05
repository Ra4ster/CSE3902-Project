using Byte.Player;
using Byte.Projectile;
using Microsoft.Xna.Framework;
using System;

namespace Byte.Command
{
    // spawns a projectile in front of link, facing the way link faces
    public class UseProjectileCommand : ICommand
    {
        // the controller fires every frame a key is held, so limit how often items can be used
        private static readonly TimeSpan COOLDOWN = TimeSpan.FromSeconds(0.3);

        // link's position is near his feet, move up to the middle of his body
        private static readonly Vector2 LINK_CENTER_OFFSET = new Vector2(0, -30);
        private const float SPAWN_DISTANCE = 60f;

        private readonly Link player;
        private readonly ProjectileManager projectileManager;
        private readonly Func<Vector2, Vector2, IProjectile> createProjectile;
        private TimeSpan? lastUsed;

        // createProjectile takes a spawn position and a screen-space direction
        public UseProjectileCommand(Link link, ProjectileManager projectileManager, Func<Vector2, Vector2, IProjectile> createProjectile)
        {
            player = link;
            this.projectileManager = projectileManager;
            this.createProjectile = createProjectile;
        }

        public void Execute(GameTime gameTime)
        {
            if (lastUsed.HasValue && gameTime.TotalGameTime - lastUsed.Value < COOLDOWN)
            {
                return;
            }
            lastUsed = gameTime.TotalGameTime;

            // CardinalDirections.North is +Y, but on screen up is -Y
            player.UseItem();
            Vector2 direction = new Vector2(player.Direction.X, -player.Direction.Y);
            Vector2 spawnPosition = player.Position + LINK_CENTER_OFFSET + direction * SPAWN_DISTANCE;
            projectileManager.Add(createProjectile(spawnPosition, direction));
        }
    }
}
