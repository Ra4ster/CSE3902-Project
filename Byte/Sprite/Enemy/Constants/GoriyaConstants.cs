
using Microsoft.Xna.Framework;

namespace Byte.Sprite.Enemy
{
    internal static partial class EnemyConstants
    {
        internal static class Goriya
        {
            public static readonly Rectangle DOWN_FRAME = new(223, 12, 15, 15);
            public static readonly Rectangle UP_FRAME = new(240, 12, 15, 15);
            public static readonly Rectangle[] SIDE_FRAMES =
            [
                new(257, 12, 15, 15),
            new(274, 12, 15, 15)
            ];
            public static readonly Rectangle[] ALL_FRAMES =
            [
                DOWN_FRAME,
            UP_FRAME,
            SIDE_FRAMES[0],
            SIDE_FRAMES[1]
            ];
            public static readonly Vector2[] REL_PATHS = Stalfos.REL_PATHS;
            public static readonly float SPEED = 150f;
            public static readonly float WAIT_DURATION = 0.5f;
            public static readonly float WALK_FLIP_INTERVAL = 0.15f;
            public static readonly float THROW_INTERVAL = 3f;
        }
    }
}
