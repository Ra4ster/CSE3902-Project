using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Stalfos : PatrolEnemy
    {
        private static readonly Rectangle []sourceRect = { new Rectangle(1, 59, 15, 15) };

        internal Stalfos(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float frameDuration, float scale, Vector2[] patrolPath, float patrolSpeed) : base(texture, ref position, ref velocity, color, sourceRect, frameDuration, scale, patrolPath, patrolSpeed)
        {
        }
    }
}
