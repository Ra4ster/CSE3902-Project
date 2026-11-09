using Byte.Rooms;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Core.Rooms
{
    internal class DoorFactory
    {
        private readonly Texture2D blockSheet;
        public DoorFactory(Texture2D blockSheet)
        {
            this.blockSheet = blockSheet;
        }

        public Door CreateDoor(DoorData data)
        {
            DoorDirection direction = Enum.TryParse<DoorDirection>(data.Direction, true, out var parsedDir)
                ? parsedDir
                : DoorDirection.Default;

            Door door = new Door(data);
            StaticSprite sprite = CreateDoorSprite(direction, door.doorState, data.Position);

            door.sprite = sprite;
            return door;
        }

        public DoorWall CreateDoorWall(DoorDirection direction, GridPosition position)
        {
            return new DoorWall(CreateDoorSprite(direction, DoorState.Wall, position));
        }

        private static readonly Rectangle WallDoorNFrame = new(815, 11, 32, 32);
        private static readonly Rectangle OpenDoorNFrame = new(848, 11, 32, 32);
        private static readonly Rectangle LockedDoorNFrame = new(881, 11, 32, 32);
        private static readonly Rectangle SealedDoorNFrame = new(914, 11, 32, 32);
        private static readonly Rectangle BlownDoorNFrame = new(947, 11, 32, 32);

        private StaticSprite CreateDoorSprite(DoorDirection direction, DoorState state, GridPosition gridPosition)
        {
            const int stride = 33;

            Rectangle relFrame = (state) switch
            {
                DoorState.Wall => WallDoorNFrame,
                DoorState.Open => OpenDoorNFrame,
                DoorState.Locked => LockedDoorNFrame,
                DoorState.Sealed => SealedDoorNFrame,
                DoorState.Blown => BlownDoorNFrame,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, "Unsupported door state.")
            };
            Rectangle absFrame = (direction) switch
            {
                DoorDirection.West => new Rectangle(relFrame.X, relFrame.Y + stride, relFrame.Width, relFrame.Height),
                DoorDirection.East => new Rectangle(relFrame.X, relFrame.Y + stride * 2, relFrame.Width, relFrame.Height),
                DoorDirection.South => new Rectangle(relFrame.X, relFrame.Y + stride * 3, relFrame.Width, relFrame.Height),
                _ => relFrame
            };

            return new StaticSprite(
                blockSheet,
                GetDoorSpritePosition(direction, gridPosition),
                Color.White,
                absFrame,
                GameConstants.SCALE
            );
        }

        private static Vector2 GetDoorSpritePosition(DoorDirection direction, GridPosition gridPosition)
        {
            Vector2 position = RoomGrid.ToWorld(gridPosition);
            float tileSize = RoomGrid.TILE_WORLD_SIZE;
            return position + (direction switch
            {
                DoorDirection.North => new Vector2(-tileSize + GameConstants.SCALE, -2 * tileSize),
                DoorDirection.South => new Vector2(-tileSize + GameConstants.SCALE, tileSize),
                DoorDirection.East => new Vector2(tileSize + 1, -tileSize / 2f),
                DoorDirection.West => new Vector2(-2 * tileSize + GameConstants.SCALE - 1, -tileSize / 2f),
                _ => Vector2.Zero
            });
        }

    }
}