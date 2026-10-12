
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class BladeTrap : MechanicalEnemy
    {
        internal BladeTrap(Texture2D texture, ref Vector2 homePosition, Vector2[] endPositions,
            ref Vector2 velocity, Color color, float movementSpeed, float? scale)
            : base(texture, ref homePosition, endPositions, ref velocity, color,
            EnemyConstants.BladeTrap.SOURCE_RECT, movementSpeed,
            scale ?? GameConstants.SCALE)
        {
        }

        protected override float GetMovementSpeed(bool goingHome, float currentSpeed, float elapsedSeconds)
        {
            float targetSpeed = goingHome ? MovementSpeed * EnemyConstants.BladeTrap.RETURN_SPEED_FACTOR : MovementSpeed;
            float lerpAmount = MathHelper.Clamp(EnemyConstants.BladeTrap.ACCELERATION_RATE * elapsedSeconds, 0f, 1f);
            return MathHelper.Lerp(currentSpeed, targetSpeed, lerpAmount);
        }
    }
}