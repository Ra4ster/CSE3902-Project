
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

    struct EnemyDescription
    {
        public EnemyType Type { get; set; }
        public Vector2[]? Paths { get; set; }

        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public float FrameDuration { get; set; }
        public float WaitDuration { get; set; }
        public float Scale { get; set; }
    }

    internal class EnemyFactory
    {
        private const float FRAME_DURATION = 0.08f;

        private EnemyFactory() { }

        private static EnemyFactory instance = new EnemyFactory();

        /// <summary>
        /// Eager initialization; we can assume the game has enemies.
        /// </summary>
        public static EnemyFactory Instance => instance;

        public AbstractEnemy CreateEnemy(ref EnemyDescription desc, ProjectileManager? projectileManager)
        {
            Texture2D enemyTex = GameAssets.Instance.EnemySheet;
            Texture2D bossTex = GameAssets.Instance.BossSheet;
            ProjectileFactory projectileFactory = new ProjectileFactory(GameAssets.Instance.LinkSheet, bossTex);

            float frameDuration = desc.FrameDuration == 0f ? FRAME_DURATION : desc.FrameDuration;
            float scale = desc.Scale == 0f ? GameConstants.SCALE : desc.Scale;
            Vector2 velocity = Vector2.Zero;
            Vector2 position = desc.Paths is { Length: > 0 } path
                ? path[0]
                : desc.Position;
            Vector2 bladeTrapHomePosition = desc.Position;
            Action<Vector2, Vector2> spawnFireball = (fireballPosition, direction) =>
                projectileManager!.Add(projectileFactory.CreateFireball(fireballPosition, direction));

            Func<Vector2, Vector2, IProjectile> throwBoomerang = (boomerangPosition, direction) =>
            {
                IProjectile thrown = projectileFactory.CreateBoomerang(boomerangPosition, direction);
                projectileManager!.Add(thrown);
                return thrown;
            };

            return desc.Type switch
            {
                EnemyType.Stalfos => new Stalfos(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Paths!, desc.Speed),
                EnemyType.Wallmaster => new Wallmaster(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Paths!, desc.Speed),
                EnemyType.Keese => new Keese(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Speed),
                EnemyType.Gel => new Gel(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Paths!, desc.Speed, desc.WaitDuration),
                EnemyType.Goriya => new Goriya(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Paths!, desc.Speed, desc.WaitDuration, throwBoomerang),
                EnemyType.Aquamentus => new Aquamentus(bossTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Paths!, desc.Speed, desc.WaitDuration, spawnFireball),
                EnemyType.BladeTrap => new BladeTrap(enemyTex, ref bladeTrapHomePosition, desc.Paths!, ref velocity, Color.White, desc.Speed, scale),
                _ => throw new ArgumentException($"Enemy type {desc.Type} is not supported by EnemyFactory.")
            };
        }
    }
}
