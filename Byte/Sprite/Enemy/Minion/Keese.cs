using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class Keese : WanderingEnemy
    {
        private float elapsedSeconds;

        internal Keese(
            Texture2D texture,
            ref Vector2 position,
            ref Vector2 velocity,
            Color color,
            float? frameDuration,
            float? scale,
            float speed)
    : base(
        texture,
        ref position,
        ref velocity,
        color,
        EnemyConstants.Keese.SOURCE_RECTS,
        frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION,
        scale ?? GameConstants.SCALE,
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
                elapsedSeconds / EnemyConstants.Keese.RAMP_DURATION_SECONDS,
                0f,
                1f);

	    if (progress == 1f)
		    progress = 0f;
            return patrolSpeed * MathHelper.Lerp(
                EnemyConstants.Keese.SPEED_MULTIPLIER,
                EnemyConstants.Keese.MAX_SPEED_MULTIPLIER,
                progress);
        }
    }
}
