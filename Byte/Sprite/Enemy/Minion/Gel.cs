using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite.Enemy.Minion
{
    internal class Gel : WaitingPatrolEnemy
    {
        internal Gel(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color, float? frameDuration, float? scale, Vector2[]? patrolPath, float patrolSpeed, float? waitingDuration)
            : base(texture, ref position, ref velocity, color, EnemyConstants.Gel.SOURCE_RECTS,
            frameDuration ?? EnemyConstants.DEFAULT_FRAME_DURATION,
            scale ?? GameConstants.SCALE,
            patrolPath ?? EnemyConstants.Gel.REL_PATHS, patrolSpeed,
            waitingDuration ?? EnemyConstants.Gel.WAIT_DURATION)
        {
        }
    }
}