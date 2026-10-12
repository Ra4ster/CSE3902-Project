using Microsoft.Xna.Framework;

namespace Byte.Sprite.Enemy
{
    internal static partial class EnemyConstants
    {
        internal static class Stalfos
        {
            public static readonly Rectangle SOURCE_RECT = new(2, 59, 15, 16);
            public static readonly float FLIP_INTERVAL = 0.25f;
            public static readonly float SPEED = 200f;
            public static readonly Vector2[] REL_PATHS =
            [
                new(GameConstants.WINDOW_SIZE.Left, GameConstants.WINDOW_SIZE.Top),
            new(GameConstants.WINDOW_SIZE.Right, GameConstants.WINDOW_SIZE.Top),
            new(GameConstants.WINDOW_SIZE.Left, GameConstants.WINDOW_SIZE.Bottom),
            new(GameConstants.WINDOW_SIZE.Right, GameConstants.WINDOW_SIZE.Bottom),
        ];
        }
    }
}
