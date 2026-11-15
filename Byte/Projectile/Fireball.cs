using Microsoft.Xna.Framework;

namespace Byte.Projectile
{
    public class Fireball : AbstractProjectile
    {
        private const float SPEED = 300f;
        private const float MAX_DISTANCE = 700f;
        private const float SCALE = 6.0f;

        private readonly MovingAnimatedSprite movingSprite;
        private float distanceTraveled;

        const int posX = 10;
        const int posY = 8;
        public Fireball(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
            : base(
                sprite,
               direction.X != 0 ? new Point((int)(posX * SCALE), (int)(posY * SCALE)) : new Point((int)(posX * SCALE), (int)(posY * SCALE)),
                position,
                Vector2.Normalize(direction) * SPEED,
                EnemyProjectileProperties
            )
        {
            this.movingSprite = sprite;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            distanceTraveled += SPEED * seconds;

            if (distanceTraveled >= MAX_DISTANCE || !GameConstants.WINDOW_SIZE.Contains(Position.ToPoint()))
            {
                IsExpired = true;
            }

            movingSprite.SetPos(Position);
        }
    }
}