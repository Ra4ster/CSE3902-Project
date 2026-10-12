using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy
{
    internal class Wallmaster : PatrolEnemy
    {
        internal Wallmaster(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float? frameDuration, float? scale, Vector2[]? patrolPath, float patrolSpeed)
            : base(texture, ref position, ref velocity, color, EnemyConstants.Wallmaster.SOURCE_RECTS,
            frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION,
            scale ?? GameConstants.SCALE,
            patrolPath ?? EnemyConstants.Wallmaster.REL_PATHS, patrolSpeed)
        {
        }
    }
}