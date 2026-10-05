using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Gel : WaitingPatrolEnemy
    {
        private static readonly Rectangle[] sourceRects = {
    new Rectangle(2, 12, 6, 14),
    new Rectangle(11, 12, 6, 14)
};

        internal Gel(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed, float waitingDuration)
            : base(texture, ref position, ref velocity, color, sourceRects, frameDuration, scale, patrolPath, patrolSpeed, waitingDuration)
        {
        }
    }
}