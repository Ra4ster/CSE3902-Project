using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    /// <summary>
    /// An enemy that wanders toward randomly selected positions.
    /// </summary>
    internal class WanderingEnemy : AbstractEnemy
    {
        protected float patrolSpeed { get; }

        private Vector2 targetPosition;

        private static readonly Random random = new();

        internal WanderingEnemy(
            Texture2D texture,
            ref Vector2 position,
            ref Vector2 velocity,
            Color color,
            Rectangle[] sourceRectangles,
            float frameDuration,
            float scale,
            float patrolSpeed)
            : base(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale)
        {
            this.patrolSpeed = patrolSpeed;
            PickNewTarget();
        }

        protected virtual float GetPatrolSpeed(GameTime gameTime)
        {
            return patrolSpeed;
        }

        protected override void Move(GameTime gameTime)
        {
            float speed = GetPatrolSpeed(gameTime);

            Vector2 direction = targetPosition - position;

            if (direction.LengthSquared() < 25f)
            {
                PickNewTarget();
                return;
            }

            direction.Normalize();
            velocity = direction * speed;
        }

        private void PickNewTarget()
        {
            targetPosition = ClampToWindow(position + new Vector2(
                random.Next(-150, 151),
                random.Next(-150, 151)
            ));
        }
    }
}