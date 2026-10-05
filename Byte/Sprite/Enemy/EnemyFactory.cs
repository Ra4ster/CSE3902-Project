
using Byte.Projectile;
using Byte.Sprite.Enemy.Minion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    public enum EnemyType
    {
        Stalfos,
        Keese,
        Gel,
        Aquamentus
    }

    struct EnemyDescription
    {
        public EnemyType Type { get; set; }
        public Vector2[]? PatrolPath { get; set; }
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
            Vector2 position = desc.PatrolPath is { Length: > 0 } path
                ? path[0]
                : desc.Position;
            Action<Vector2, Vector2> spawnFireball = (fireballPosition, direction) =>
                projectileManager!.Add(projectileFactory.CreateFireball(fireballPosition, direction));

            return desc.Type switch
            {
                EnemyType.Stalfos => new Stalfos(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.PatrolPath!, desc.Speed),
                EnemyType.Keese => new Keese(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.Speed),
                EnemyType.Gel => new Gel(enemyTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.PatrolPath!, desc.Speed, desc.WaitDuration),
                EnemyType.Aquamentus => new Aquamentus(bossTex, ref position, ref velocity, Color.White, frameDuration, scale, desc.PatrolPath!, desc.Speed, desc.WaitDuration, spawnFireball),
                _ => throw new ArgumentException($"Enemy type {desc.Type} is not supported by EnemyFactory.")
            };
        }
    }
}
