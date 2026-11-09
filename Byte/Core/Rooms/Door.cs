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

        public bool IsOpen { get; private set; } = true;

        private StaticSprite? openSprite;
        private StaticSprite? closedSprite;

        public CollisionProperties Properties { get; private set; } = new CollisionProperties
        {
            Type = CollidableTypes.Door,
            IsStatic = true
        };

        public string TargetRoom => Data.TargetRoom;

        public Door(DoorData data)
        {
            Data = data;

            if (Enum.TryParse<DoorDirection>(data.Direction, true, out var parsedDir))
            {
                doorDirection = parsedDir;
            }
            else
            {
                doorDirection = DoorDirection.Default;
            }

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

        public void SetSprites(StaticSprite open, StaticSprite closed)
        {
            openSprite = open;
            closedSprite = closed;
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
            StaticSprite? spriteToDraw = IsOpen ? openSprite : closedSprite;

            if (spriteToDraw != null)
            {
                spriteToDraw.Draw(painter);
            }
        }
    }
}