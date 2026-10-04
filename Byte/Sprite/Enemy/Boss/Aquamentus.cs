
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Byte.Sprite.Enemy
{
    internal class Aquamentus : WaitingPatrolEnemy
    {
        private static Rectangle[] sourceRects =
        {
            new Rectangle(2, 12, 23, 31),
            new Rectangle(27, 12, 23, 31)
        };

        internal Aquamentus(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed, float waitDuration)
            : base(texture, ref position, ref velocity, color, sourceRects, frameDuration, scale, patrolPath, patrolSpeed, waitDuration) { }
    }
}