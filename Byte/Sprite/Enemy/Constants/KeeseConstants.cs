using Microsoft.Xna.Framework;

namespace Byte.Sprite.EnemyConstants
{
    internal static partial class EnemyConstants
    {
        internal static class Keese
        {
            public static readonly float RAMP_DURATION_SECONDS = 2f;
            public static readonly float SPEED = 200f;
            public static readonly float SPEED_MULTIPLIER = 0.1f;
            public static readonly float MAX_SPEED_MULTIPLIER = 2f;
            public static readonly Rectangle[] SOURCE_RECTS =
            [
                new(184, 12, 14, 14),
            new(201, 12, 14, 14)
            ];
        }
    }
}