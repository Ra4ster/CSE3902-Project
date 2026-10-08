
using Byte.Projectile;
using Byte.Sprite.Enemy.Minion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    public enum EnemyType
    {
        Wallmaster,
        Stalfos,
        Keese,
        Gel,
        Goriya,
        BladeTrap,
        Aquamentus
    }

    public static class EnemyFactory
    {
        /// <param name="at">Initial world position.</param>
        /// <param name="path">
        /// Optional route offsets. Patrol routes are translated so their first point is <paramref name="at"/>;
        /// mechanical-enemy paths are endpoint offsets from <paramref name="at"/>.
        /// </param>
        public static AbstractEnemy CreateEnemy(
            EnemyType type,
            Vector2 at,
            ProjectileManager? projectileManager = null,
            Vector2[]? path = null,
            float? speed = null,
            float? frameDuration = null,
            float? waitDuration = null,
            float? scale = null)
        {
            if (type is EnemyType.Goriya or EnemyType.Aquamentus)
                RequireProjectileManager(projectileManager, type);

            Texture2D enemyTex = GameAssets.Instance.EnemySheet;
            Texture2D bossTex = GameAssets.Instance.BossSheet;
            Vector2 velocity = Vector2.Zero;
            float resolvedScale = scale ?? GameConstants.SCALE;
            float resolvedFrameDuration = frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION;
            Vector2[]? defaultPath = type switch
            {
                EnemyType.Stalfos => EnemyConstants.Stalfos.REL_PATHS,
                EnemyType.Wallmaster => EnemyConstants.Wallmaster.REL_PATHS,
                EnemyType.Gel => EnemyConstants.Gel.REL_PATHS,
                EnemyType.Goriya => EnemyConstants.Goriya.REL_PATHS,
                EnemyType.Aquamentus => EnemyConstants.Aquamentus.REL_PATHS,
                _ => null
            };
            Vector2[]? patrolPath = path is not null
                ? TranslatePatrolPath(at, path)
                : defaultPath is not null
                    ? TranslatePatrolPath(at, defaultPath)
                    : null;
            ProjectileFactory? projectileFactory = null;
            Action<Vector2, Vector2> spawnFireball = (fireballPosition, direction) =>
            {
                ProjectileManager manager = RequireProjectileManager(projectileManager, type);
                ProjectileFactory factory = projectileFactory ??=
                    new ProjectileFactory(GameAssets.Instance.LinkSheet, bossTex);
                manager.Add(factory.CreateFireball(fireballPosition, direction));
            };

            Func<Vector2, Vector2, IProjectile> throwBoomerang = (boomerangPosition, direction) =>
            {
                ProjectileManager manager = RequireProjectileManager(projectileManager, type);
                ProjectileFactory factory = projectileFactory ??=
                    new ProjectileFactory(GameAssets.Instance.LinkSheet, bossTex);
                IProjectile thrown = factory.CreateBoomerang(boomerangPosition, direction);
                manager.Add(thrown);
                return thrown;
            };

            return type switch
            {
                EnemyType.Stalfos => new Stalfos(enemyTex, ref at, ref velocity, Color.White,
                    resolvedFrameDuration, resolvedScale, patrolPath, speed ?? EnemyConstants.Stalfos.SPEED),
                EnemyType.Wallmaster => new Wallmaster(enemyTex, ref at, ref velocity, Color.White,
                    frameDuration ?? EnemyConstants.Wallmaster.FRAME_DURATION, resolvedScale, patrolPath,
                    speed ?? EnemyConstants.Wallmaster.SPEED),
                EnemyType.Keese => new Keese(enemyTex, ref at, ref velocity, Color.White,
                    resolvedFrameDuration, resolvedScale, speed ?? EnemyConstants.Keese.SPEED),
                EnemyType.Gel => new Gel(enemyTex, ref at, ref velocity, Color.White,
                    resolvedFrameDuration, resolvedScale, patrolPath, speed ?? EnemyConstants.Gel.SPEED,
                    waitDuration ?? EnemyConstants.Gel.WAIT_DURATION),
                EnemyType.Goriya => new Goriya(enemyTex, ref at, ref velocity, Color.White,
                    resolvedFrameDuration, resolvedScale, patrolPath, speed ?? EnemyConstants.Goriya.SPEED,
                    waitDuration ?? EnemyConstants.Goriya.WAIT_DURATION, throwBoomerang),
                EnemyType.Aquamentus => new Aquamentus(bossTex, ref at, ref velocity, Color.White,
                    frameDuration ?? EnemyConstants.Aquamentus.FRAME_DURATION, resolvedScale, patrolPath,
                    speed ?? EnemyConstants.Aquamentus.SPEED,
                    waitDuration ?? EnemyConstants.Aquamentus.WAIT_DURATION, spawnFireball),
                EnemyType.BladeTrap => new BladeTrap(enemyTex, ref at,
                    TranslateEndpoints(at, path ?? EnemyConstants.BladeTrap.REL_PATHS), ref velocity,
                    Color.White, speed ?? EnemyConstants.BladeTrap.SPEED, resolvedScale),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported enemy type.")
            };
        }

        private static Vector2[] TranslatePatrolPath(Vector2 at, Vector2[] path)
        {
            if (path.Length == 0)
                throw new ArgumentException("A patrol path must contain at least one point.", nameof(path));

            Vector2 origin = path[0];
            return Array.ConvertAll(path, point => at + (point - origin));
        }

        private static Vector2[] TranslateEndpoints(Vector2 at, Vector2[] offsets)
        {
            if (offsets.Length == 0)
                throw new ArgumentException("A mechanical enemy must have at least one endpoint.", nameof(offsets));

            return Array.ConvertAll(offsets, offset => at + offset);
        }

        private static ProjectileManager RequireProjectileManager(ProjectileManager? projectileManager, EnemyType type) =>
            projectileManager ?? throw new ArgumentNullException(
                nameof(projectileManager), $"{type} requires a projectile manager.");
    }
}
