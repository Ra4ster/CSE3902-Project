
using Byte.Sprite.Enemy.Minion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal static class EnemyFactory
    {
        private const float SCALE = 10.0f;
        private const float FRAME_DURATION = 0.08f;

        public static AbstractEnemy CreateStalfos(
            Texture2D texture,
            Vector2[] patrolPath,
            float speed)
        {
            Vector2 velocity = Vector2.Zero;
            return new Stalfos(texture, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed);
        }

        public static AbstractEnemy CreateKeese(Texture2D texture,
        Vector2[] patrolPath, float speed)
        {
            Vector2 velocity = Vector2.Zero;
            return new Keese(texture, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed);
        }

        public static AbstractEnemy CreateGel(Texture2D texture, Vector2[] patrolPath, float speed, float waitDuration)
        {
            Vector2 velocity = Vector2.Zero;
            return new Gel(texture, ref patrolPath[0], ref velocity, Color.White, FRAME_DURATION, SCALE, patrolPath, speed, waitDuration);
        }
    }
}