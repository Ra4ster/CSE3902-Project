
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class BladeTrap : MechanicalEnemy
    {
        private const float RETURN_SPEED_FACTOR = 0.35f;
        private const float ACCELERATION_RATE = 5f;

        public static Rectangle sourceRect = new Rectangle(165, 60, 14, 14);

        internal BladeTrap(Texture2D texture, ref Vector2 homePosition, Vector2[] endPositions,
            ref Vector2 velocity, Color color, float movementSpeed, float scale)
            : base(texture, ref homePosition, endPositions, ref velocity, color, ref sourceRect, movementSpeed, scale)
        {
        }

        protected override float GetMovementSpeed(bool goingHome, float currentSpeed, float elapsedSeconds)
        {
            float targetSpeed = goingHome ? MovementSpeed * RETURN_SPEED_FACTOR : MovementSpeed;
            float lerpAmount = MathHelper.Clamp(ACCELERATION_RATE * elapsedSeconds, 0f, 1f);
            return MathHelper.Lerp(currentSpeed, targetSpeed, lerpAmount);
        }
    }
}