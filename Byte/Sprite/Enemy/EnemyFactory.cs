
using Byte.Sprite.Enemy.Minion;
using Microsoft.Xna.Framework;

namespace Byte.Sprite.Enemy
{
    internal class EnemyFactory
    {
        private const float SCALE = 10.0f;
        private const float FRAME_DURATION = 0.08f;

        private EnemyFactory() { }

        private static EnemyFactory instance = new EnemyFactory();

        /// <summary>
        /// Eager initialization; we can assume the game has enemies.
        /// </summary>
        public static EnemyFactory Instance
        {
            get
            {
                return instance;
            }
        }

        public AbstractEnemy CreateStalfos(
            Vector2[] patrolPath,
            float speed)
        {
            Vector2 velocity = Vector2.Zero;
            return new Stalfos(GameAssets.Instance.EnemySheet, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed);
        }

        public AbstractEnemy CreateKeese(Vector2[] patrolPath, float speed)
        {
            Vector2 velocity = Vector2.Zero;
            return new Keese(GameAssets.Instance.EnemySheet, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed);
        }

        public AbstractEnemy CreateGel(Vector2[] patrolPath, float speed, float waitDuration)
        {
            Vector2 velocity = Vector2.Zero;
            return new Gel(GameAssets.Instance.EnemySheet, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed, waitDuration);
        }

        public AbstractEnemy CreateAquamentus(Vector2[] patrolPath, float speed, float waitDuration)
        {
            Vector2 velocity = Vector2.Zero;
            return new Aquamentus(GameAssets.Instance.BossSheet, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed, waitDuration);
        }
    }
}
