using Byte.Item;
using Byte.Player;
using Byte.Projectile;
using Microsoft.Xna.Framework;

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
        private readonly Func<IProjectile, bool> canUseItemCallback;
        private readonly Func<Vector2, Vector2, IProjectile> createProjectile;

        public static event Action<IProjectile> OnProjectileFired;
        private TimeSpan? lastUsed;

        // createProjectile takes a spawn position and a screen-space direction
        public UseProjectileCommand(
                    Link link,
                    ProjectileManager projectileManager,
                    Func<Vector2, Vector2, IProjectile> createProjectile,
                    Func<IProjectile, bool> canUseItemCallback)
        {
            player = link;
            this.projectileManager = projectileManager;
            this.createProjectile = createProjectile;
            this.canUseItemCallback = canUseItemCallback;
        }

        public void Execute(GameTime gameTime)
        {
            if (lastUsed.HasValue && gameTime.TotalGameTime - lastUsed.Value < COOLDOWN)
            {
                return;
            }
            // CardinalDirections.North is +Y, but on screen up is -Y
            Vector2 direction = new Vector2(player.Direction.X, -player.Direction.Y);
            Vector2 spawnPosition = player.Position + LINK_CENTER_OFFSET + direction * SPAWN_DISTANCE;
            IProjectile createdProjectile = createProjectile(spawnPosition, direction);

            if (canUseItemCallback != null && !canUseItemCallback(createdProjectile))
            {
                return;
            }

            lastUsed = gameTime.TotalGameTime;
            player.UseItem();
            projectileManager.Add(createdProjectile);

            OnProjectileFired?.Invoke(createdProjectile);
        }
    }
}
