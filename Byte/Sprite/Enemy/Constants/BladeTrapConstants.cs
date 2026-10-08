
using Microsoft.Xna.Framework;

namespace Byte.Sprite.Enemy
{
    internal static partial class EnemyConstants
    {
        internal static class BladeTrap
        {
            public static readonly float SPEED = 800f;
            public static readonly float RETURN_SPEED_FACTOR = 0.35f;
            public static readonly float ACCELERATION_RATE = 5f;
            public static readonly Rectangle SOURCE_RECT = new(165, 60, 14, 14);
            public static readonly Vector2[] REL_PATHS =
            [
                new(-GameConstants.WINDOW_SIZE.Width, 0)
            ];
        }
    }
}
