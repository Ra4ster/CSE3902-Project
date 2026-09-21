using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Stalfos : PatrolEnemy
    {
        private static readonly Rectangle[] sourceRect = { new Rectangle(2, 59, 15, 15) };

        private float flipTimer = 0f;
        private const float FLIP_INTERVAL = 0.25f; // Flip every 0.25 seconds
        private SpriteEffects spriteEffect = SpriteEffects.None;

        internal Stalfos(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed)
            : base(texture, ref position, ref velocity, color, sourceRect, frameDuration, scale, patrolPath, patrolSpeed)
        { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            flipTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flipTimer >= FLIP_INTERVAL)
            {
                spriteEffect = (spriteEffect == SpriteEffects.None)
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                flipTimer = 0f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = new Vector2(
                MathF.Round(position.X),
                MathF.Round(position.Y)
            );

            spriteBatch.Draw(
                texture,
                drawPosition,
                sourceRect[0],
                color,
                0f,
                Vector2.Zero,
                scale,
                spriteEffect,
                0f
            );
        }
    }
}