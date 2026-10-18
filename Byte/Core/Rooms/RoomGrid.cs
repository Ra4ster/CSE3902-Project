
using Byte.Factory;
using Microsoft.Xna.Framework;

namespace Byte.Rooms
{
    public static class RoomGrid
    {
        public const int TILE_SIZE = 16;
        public const float BLOCK_SCALE = 6.0f;
        public const float TILE_WORLD_SIZE = TILE_SIZE * BLOCK_SCALE;

        public static Vector2 ToWorld(GridPosition position) => new(position.X * TILE_WORLD_SIZE, position.Y * TILE_WORLD_SIZE);

    }
}