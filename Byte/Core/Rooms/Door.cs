using Byte.Collidable;
using Byte.Core.Rooms;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Rooms
{
    public sealed class Door : ICollidable, Byte.Drawing.IDrawable
    {
        public DoorDirection doorDirection;
        public DoorState doorState;
        public DoorData Data { get; }
        public Rectangle BoundingBox { get; }
        public bool CanEnter = false;

        public bool IsOpen { get; private set; } = true;

        public StaticSprite? sprite;

        public CollisionProperties Properties { get; private set; } = new CollisionProperties
        {
            Type = CollidableTypes.Door,
            IsStatic = true
        };

        public string TargetRoom => Data.TargetRoom;

        public Door(DoorData data)
        {
            Data = data;
            doorDirection = (Enum.TryParse<DoorDirection>(data.Direction, true, out var parsedDir)) ? parsedDir : DoorDirection.Default;
            doorState = data.Type;

            Vector2 topLeft = RoomGrid.ToWorld(data.Position);
            int side = (int)RoomGrid.TILE_WORLD_SIZE;
            int x = (int)topLeft.X;
            int y = (int)topLeft.Y;
            switch (doorDirection)
            {
                case DoorDirection.North:
                    x -= side / 2;
                    y -= 2 * side;
                    break;
                case DoorDirection.South:
                    x -= side / 2;
                    y += 2 * side;
                    break;
                case DoorDirection.East:
                    x += 2 * side;
                    break;
                case DoorDirection.West:
                    x -= 2 * side;
                    break;
            }

            BoundingBox = new Rectangle(x, y, side, side);
        }

        public void SetState(DoorState state)
        {
            this.doorState = state;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Draw(SpriteBatch painter)
        {
            sprite!.Draw(painter);
        }
    }
}