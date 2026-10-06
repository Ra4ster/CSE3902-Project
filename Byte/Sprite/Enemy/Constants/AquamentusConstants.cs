
using Microsoft.Xna.Framework;

namespace Byte.Sprite.EnemyConstants
{
    internal static partial class EnemyConstants
    {
        internal static class Aquamentus
        {
            public static readonly Rectangle[] SOURCE_RECTS =
            [
                new(2, 12, 23, 31),
                new(27, 12, 23, 31)
            ];
            public static readonly Vector2[] REL_PATHS =
            [
                new(GameConstants.WINDOW_SIZE.Bottom / 2f, GameConstants.WINDOW_SIZE.Right),
            new(GameConstants.WINDOW_SIZE.Bottom / 2f, GameConstants.WINDOW_SIZE.Left)
            ];
            public static readonly float SPEED = 200f;
            public static readonly float WAIT_DURATION = 1f;
            public static readonly float FRAME_DURATION = 0.5f;
            public static readonly float FIRE_INTERVAL = 2f;
            public static readonly Vector2 MOUTH_OFFSET = new(2, 8);
            public static readonly Vector2[] FIREBALL_DIRECTIONS =
            [
                new(-1, -0.3f),
            new(-1, 0),
            new(-1, 0.3f)
            ];
        }
    }
}