using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class Keese : WanderingEnemy
    {
        private const float SpeedRampDurationSeconds = 2f;
        private const float StartingSpeedMultiplier = 0.1f;
        private const float MaximumSpeedMultiplier = 2f;

        private static readonly Rectangle[] sourceRects =
        {
            new Rectangle(184, 12, 14, 14),
            new Rectangle(201, 12, 14, 14)
        };

        private float elapsedSeconds;

        internal Keese(
            Texture2D texture,
            ref Vector2 position,
            ref Vector2 velocity,
            Color color,
            float frameDuration,
            float scale,
            float speed)
    : base(
        texture,
        ref position,
        ref velocity,
        color,
        sourceRects,
        frameDuration,
        scale,
        speed)
        {
        }

        public override void Update(GameTime gameTime)
        {
            elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        protected override float GetPatrolSpeed(GameTime gameTime)
        {
            float progress = MathHelper.Clamp(
                elapsedSeconds / SpeedRampDurationSeconds,
                0f,
                1f);

            return patrolSpeed * MathHelper.Lerp(
                StartingSpeedMultiplier,
                MaximumSpeedMultiplier,
                progress);
        }
    }
}