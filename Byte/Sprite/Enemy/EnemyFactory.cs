
using Byte.Projectile;
using Byte.Sprite.Enemy.Minion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class EnemyFactory
    {
        private const float FRAME_DURATION = 0.08f;

        private EnemyFactory() { }

        private static EnemyFactory instance = new EnemyFactory();

        /// <summary>
        /// Eager initialization; we can assume the game has enemies.
        /// </summary>
        public static EnemyFactory Instance => instance;

        public AbstractEnemy Create<T>(
            Vector2[]? patrolPath = null,
            Vector2 pos = default,
            float speed = 0.0f,
            float frameDuration = FRAME_DURATION,
            float waitDuration = default,
            float scale = GameConstants.SCALE
        ) where T : AbstractEnemy
        {
            Vector2 velocity = Vector2.Zero;
            Texture2D enemySheet = GameAssets.Instance.EnemySheet;
            Texture2D bossSheet = GameAssets.Instance.BossSheet;
            ProjectileFactory projectileFactory = new ProjectileFactory(GameAssets.Instance.LinkSheet, bossSheet);

            return typeof(T).Name switch
            {
                nameof(Stalfos) => new Stalfos(enemySheet, ref patrolPath![0], ref velocity, Color.White, frameDuration, scale, patrolPath!, speed),
                nameof(Keese) => new Keese(enemySheet, ref pos, ref velocity!, Color.White, frameDuration, scale, speed),
                nameof(Gel) => new Gel(enemySheet, ref patrolPath![0], ref velocity!, Color.White, frameDuration, scale, patrolPath!, speed, waitDuration!),
                nameof(Aquamentus) => new Aquamentus(bossSheet, ref patrolPath![0], ref velocity, Color.White, frameDuration, scale, patrolPath!, speed, waitDuration!, projectileFactory),
                _ => throw new ArgumentException($"Enemy type {typeof(T).Name} is not supported by EnemyFactory.")
            };
        }
    }
}
