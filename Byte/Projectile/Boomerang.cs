using Byte.Collidable;
using Microsoft.Xna.Framework;

namespace Byte.Projectile
{
    public class Boomerang : AbstractProjectile
    {
        private const float SPEED = 400f;
        private const float RANGE = 450f;
        private const float SCALE = 6.0f;

        private readonly MovingAnimatedSprite movingSprite;
        private Vector2 startPosition;
        private float distanceTraveled;
        private bool isReturning = false;

        const int startPosX = 8;
        const int startPosY = 16;
        // Player boomerang
        public Boomerang(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction)
            : base(
                sprite,
                new Point((int)(startPosX * SCALE), (int)(startPosY * SCALE)),
                position,
                direction * SPEED,
                PlayerProjectileProperties
            )
        {
            this.movingSprite = sprite;
            this.startPosition = position;
        }

        //Enemy boomerang
        public Boomerang(MovingAnimatedSprite sprite, Vector2 position, Vector2 direction, CollisionProperties properties)
            : base(
                sprite,
                new Point((int)(startPosX * SCALE), (int)(startPosY * SCALE)),
                position,
                direction * SPEED,
                properties
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
                const float maxDist = 5f;
                const float dbl = 2f;

                if (distanceToStart <= maxDist || distanceTraveled >= RANGE * dbl)
                {
                    IsExpired = true;
                    Program.soundManager.StopBoomerangSound(this);
                }
            }

            movingSprite.SetPos(Position);
        }

        new public CollisionProperties Properties
        {
            get => base.Properties;
            set => base.Properties = value;
        }
    }
}