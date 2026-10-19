using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile
{
    public class Bomb : AbstractProjectile
    {
        private const float SCALE = 6.0f;
        private const float EXPLOSION_DURATION = 0.3f;

        private readonly MovingAnimatedSprite bombSprite;
        private readonly MovingAnimatedSprite explosionSprite;

        private bool isExploding = false;
        private float explosionTimer = 0f;

        public Bomb(MovingAnimatedSprite bombSprite, MovingAnimatedSprite explosionSprite, Vector2 position)
            : base(
                bombSprite,
                new Point((int)(8 * SCALE), (int)(16 * SCALE)), 
                position,
                Vector2.Zero 
            )
        {
            this.bombSprite = bombSprite;
            this.explosionSprite = explosionSprite;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 

            if (!isExploding)
            {
                bombSprite.SetPos(Position);


                if (HasCollided)
                {
                    StartExplosion();
                }
            }
            else
            {
                float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                explosionTimer += seconds;

                explosionSprite.SetPos(Position);
                explosionSprite.Update(gameTime);

                if (explosionTimer >= EXPLOSION_DURATION)
                {
                    IsExpired = true;
                }
            }
        }

        private void StartExplosion()
        {
            isExploding = true;
        }


        public new void Draw(SpriteBatch spriteBatch)
        {
            if (IsExpired) return;

            if (!isExploding)
            {
                bombSprite.Draw(spriteBatch);
            }
            else
            {
                explosionSprite.Draw(spriteBatch);
            }
        }
    }
}