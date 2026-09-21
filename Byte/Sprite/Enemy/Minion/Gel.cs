using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Gel : WaitingPatrolEnemy
    {
        private static readonly Rectangle[] sourceRects = {
            new Rectangle(2, 11, 7, 15),
            new Rectangle(10, 11, 7, 15)
        };

        internal Gel(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed, float waitingDuration)
            : base(texture, ref position, ref velocity, color, sourceRects, frameDuration, scale, patrolPath, patrolSpeed, waitingDuration)
        {
        }
    }
}