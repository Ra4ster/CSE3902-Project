using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Projectile
{
    public class Boomerang : AbstractProjectile
    {
        private const float SPEED = 600f;
        private const float RANGE = 450f;
        private const float SCALE = 6.0f;

        private readonly MovingAnimatedSprite movingSprite;
        private Vector2 startPosition;
        private float distanceTraveled;
        private bool isReturning = false;

        public Boomerang(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
            : base(
                sprite,
                new Point((int)(8 * SCALE), (int)(16 * SCALE)), 
                position,
                direction * SPEED,
                PlayerProjectileProperties
            )
        {
            this.movingSprite = sprite;
            this.startPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 

            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            distanceTraveled += SPEED * seconds;

            if (!isReturning && distanceTraveled >= RANGE)
            {
                isReturning = true;
                Velocity = -Velocity; 
            }
            else if (isReturning)
            {
                float distanceToStart = Vector2.Distance(Position, startPosition);

                if (distanceToStart <= 5f || distanceTraveled >= RANGE * 2f)
                {
                    IsExpired = true;
                }
            }

            movingSprite.SetPos(Position);
        }
    }
}