
using Microsoft.Xna.Framework;

namespace Byte.Sprite.Enemy
{
    internal static partial class EnemyConstants
    {

        internal static class Wallmaster
        {
            public static readonly Rectangle[] SOURCE_RECTS =
            [
            	new(394, 11, 15, 16),
            	new(411, 11, 15, 16)
            ];
            public static readonly Vector2[] REL_PATHS =
            [
                new(1200, 1000),
            new(1100, 1200),
            new(1000, 1300)
            ];
            public static readonly float SPEED = 100f;
            public static readonly float FRAME_DURATION = 0.25f;
        }
    }
}
