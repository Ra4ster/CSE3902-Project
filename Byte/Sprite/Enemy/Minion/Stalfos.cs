using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Stalfos : PatrolEnemy
    {
        private float flipTimer = 0f;
        private SpriteEffects spriteEffect = SpriteEffects.None;

        internal Stalfos(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float? frameDuration, float? scale, Vector2[]? patrolPath, float patrolSpeed)
            : base(texture, ref position, ref velocity, color,
            [EnemyConstants.Stalfos.SOURCE_RECT],
            frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION,
            scale ?? GameConstants.SCALE,
            patrolPath ?? EnemyConstants.Stalfos.REL_PATHS, patrolSpeed)
        { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            flipTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flipTimer >= EnemyConstants.Stalfos.FLIP_INTERVAL)
            {
                spriteEffect = (spriteEffect == SpriteEffects.None)
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                flipTimer = 0f;

                if (enemySprite != null)
                    enemySprite.Effects = spriteEffect;
            }
        }
    }
}