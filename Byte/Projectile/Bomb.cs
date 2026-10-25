using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile
{
    public class Bomb : AbstractProjectile
    {
        private const float SCALE = 6.0f;
        private const float EXPLOSION_DURATION = 0.3f;
        private const float FUSE_DURATION = 2.0f; 

        private readonly MovingAnimatedSprite bombSprite;
        private readonly MovingAnimatedSprite explosionSprite;

        private bool isExploding = false;
        private float explosionTimer = 0f;
        private float fuseTimer = 0f;

        public Bomb(MovingAnimatedSprite bombSprite, MovingAnimatedSprite explosionSprite, Vector2 position, Vector2 velocity)
            : base(
                bombSprite,
                new Point((int)(8 * SCALE), (int)(16 * SCALE)),
                position,
                velocity, 
                PlayerProjectileProperties
            )
        {
            this.bombSprite = bombSprite;
            this.explosionSprite = explosionSprite;
        }

        public override void Update(GameTime gameTime)
        {

            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isExploding)
            {
                base.Update(gameTime);
                bombSprite.SetPos(Position);
                bombSprite.Update(gameTime);

                fuseTimer += seconds;

                if (HasCollided || fuseTimer >= FUSE_DURATION)
                {
                    StartExplosion();
                }
            }
            else
            {
                Velocity = Vector2.Zero;
                explosionTimer += seconds;

                explosionSprite.SetPos(Position);
                explosionSprite.Update(gameTime);

                if (explosionTimer >= EXPLOSION_DURATION)
                {
                    IsExpired = true;
                    HasCollided = true;
                }
            }
        }
        public override void Collided()
        {
            if (!isExploding)
            {
                StartExplosion();
            }
        }
        private void StartExplosion()
        {
            isExploding = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
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