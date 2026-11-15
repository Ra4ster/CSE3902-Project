using Microsoft.Xna.Framework;

namespace Byte.Projectile
{
    public class Arrow : AbstractProjectile
    {
        private const float SPEED = 800f;
        private const float MAX_DISTANCE = 700f;
        private const float SCALE = 6.0f;

        private readonly MovingAnimatedSprite movingSprite;
        private float distanceTraveled;

        const int startPosX = 16;
        const int startPosY = 8;

        public Arrow(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
            : base(
                sprite,
                direction.X != 0 ? new Point((int)(startPosX * SCALE), (int)(startPosY * SCALE)) : new Point((int)(startPosX * SCALE), (int)(startPosY * SCALE)),
                position,
                direction * SPEED,
                PlayerProjectileProperties
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
                IsExpired = true;

            movingSprite.SetPos(Position);
        }
    }
}