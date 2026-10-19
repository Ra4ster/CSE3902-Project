using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile
{
    public class Boomerang : AbstractProjectile
    {
        private const float SPEED = 800f;
        private const float MAX_DISTANCE = 700f;
        private const float SCALE = 6.0f;

        private readonly MovingAnimatedSprite movingSprite;
        private float distanceTraveled;

        public Boomerang(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
            : base(
                sprite,
                direction.X != 0 ? new Point((int)(16 * SCALE), (int)(8 * SCALE)) : new Point((int)(8 * SCALE), (int)(16 * SCALE)),
                position,
                direction * SPEED
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