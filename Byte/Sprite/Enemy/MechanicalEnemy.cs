
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class MechanicalEnemy : AbstractEnemy
    {
        private const float TARGET_ARRIVAL_TOLERANCE = 0.01f;

        private readonly Vector2 homePosition;
        private readonly Vector2[] endPositions;
        protected float MovementSpeed { get; }
        private int seeking;
        private bool goingHome;

        public MechanicalEnemy(Texture2D texture, ref Vector2 homePosition, Vector2[] endPositions,
            ref Vector2 velocity, Color color, ref Rectangle sourceRectangle, float movementSpeed, float scale = 1)
            : base(ref homePosition, ref velocity, sourceRectangle, scale)
        {
            if (endPositions is null || endPositions.Length == 0)
                throw new ArgumentException("At least one end position is required.", nameof(endPositions));
            if (movementSpeed <= 0f || !float.IsFinite(movementSpeed))
                throw new ArgumentOutOfRangeException(nameof(movementSpeed), "Movement speed must be finite and greater than zero.");

            enemySprite = new MovingSprite(texture, ref position, ref this.velocity, color, ref sourceRectangle, scale);
            this.homePosition = position;
            this.endPositions = Array.ConvertAll(endPositions, ClampToWindow);
            MovementSpeed = movementSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            Move(gameTime);

            MovingSprite movingSprite = (MovingSprite)enemySprite;
            movingSprite.SetPos(position);
            movingSprite.Velocity = velocity;
        }

        protected override void Move(GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime <= TimeSpan.Zero)
                return;

            Vector2 target = goingHome ? homePosition : endPositions[seeking];
            Vector2 offset = target - position;
            float distance = offset.Length();

            if (distance <= TARGET_ARRIVAL_TOLERANCE)
            {
                position = target;
                if (goingHome)
                {
                    seeking = (seeking + 1) % endPositions.Length;
                    goingHome = false;
                }
                else
                {
                    goingHome = true;
                }

                return;
            }

            if (distance <= 0f)
                return;

            Vector2 direction = offset / distance;
            float speed = GetMovementSpeed(goingHome, velocity.Length(), (float)gameTime.ElapsedGameTime.TotalSeconds);
            float travelDistance = Math.Min(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, distance);
            position = ClampToWindow(position + direction * travelDistance);
            velocity = direction * speed;
        }

        protected virtual float GetMovementSpeed(bool goingHome, float currentSpeed, float elapsedSeconds) => MovementSpeed;
    }
}