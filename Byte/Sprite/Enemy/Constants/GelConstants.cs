using Microsoft.Xna.Framework;

namespace Byte.Sprite.Enemy
{
    internal static partial class EnemyConstants
    {
        internal static class Gel
        {
            public static readonly Rectangle[] SOURCE_RECTS =
            [
                new(2, 12, 6, 14),
                new(11, 12, 6, 14)
            ];
            public static readonly Vector2[] REL_PATHS = Stalfos.REL_PATHS;
            public static readonly float SPEED = 200f;
            public static readonly float WAIT_DURATION = 1.5f;
        }
    }
}
