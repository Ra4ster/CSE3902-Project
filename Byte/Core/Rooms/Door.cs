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
        public DoorState State { get; private set; }
        public DoorData Data { get; }
        public Rectangle BoundingBox { get; }
        public bool CanEnter => State is DoorState.Open or DoorState.Blown;

        private readonly Func<DoorState, StaticSprite> spriteFactory;
        private StaticSprite sprite;
        const int dbl = 2;
        public CollisionProperties Properties { get; private set; } = new CollisionProperties
        {
            Type = CollidableTypes.Door,
            IsStatic = true
        };

        public string TargetRoom => Data.TargetRoom;

        public Door(DoorData data, Func<DoorState, StaticSprite> spriteFactory)
        {
            Data = data;
            this.spriteFactory = spriteFactory;
            doorDirection = (Enum.TryParse<DoorDirection>(data.Direction, true, out var parsedDir)) ? parsedDir : DoorDirection.Default;
            State = data.Type;
            sprite = spriteFactory(State);

            Vector2 topLeft = RoomGrid.ToWorld(data.Position);
            int side = (int)RoomGrid.TILE_WORLD_SIZE;
            int x = (int)topLeft.X;
            int y = (int)topLeft.Y;
            switch (doorDirection)
            {
                case DoorDirection.North:
                    x -= side / dbl;
                    y -= dbl * side;
                    break;
                case DoorDirection.South:
                    x -= side / dbl;
                    y += dbl * side;
                    break;
                case DoorDirection.East:
                    x += dbl * side;
                    break;
                case DoorDirection.West:
                    x -= dbl * side;
                    break;
            }

            BoundingBox = new Rectangle(x, y, side, side);
        }

        public void SetState(DoorState state)
        {
            State = state;
            sprite = spriteFactory(state);
        }

        public bool TryOpen()
        {
            if (State != DoorState.Sealed)
                return false;

            SetState(DoorState.Open);
            return true;
        }

        public bool TryUnlock()
        {
            if (State != DoorState.Locked)
                return false;

            SetState(DoorState.Open);
            return true;
        }

        public bool TryBlowOpen()
        {
            if (State != DoorState.Wall)
                return false;

            SetState(DoorState.Blown);
            return true;
        }

        public Rectangle InteractionBounds(Rectangle roomBounds)
        {
            int tileSize = (int)RoomGrid.TILE_WORLD_SIZE;
            Vector2 doorPosition = RoomGrid.ToWorld(Data.Position);
            return doorDirection switch
            {
                DoorDirection.North => new Rectangle(
                    (int)doorPosition.X - tileSize,
                    roomBounds.Top,
                    dbl * tileSize,
                    tileSize),
                DoorDirection.South => new Rectangle(
                    (int)doorPosition.X - tileSize,
                    roomBounds.Bottom - tileSize,
                    dbl * tileSize,
                    tileSize),
                DoorDirection.West => new Rectangle(
                    roomBounds.Left,
                    (int)doorPosition.Y - tileSize / dbl,
                    tileSize,
                    dbl * tileSize),
                DoorDirection.East => new Rectangle(
                    roomBounds.Right - tileSize,
                    (int)doorPosition.Y - tileSize / dbl,
                    tileSize,
                    dbl * tileSize),
                _ => Rectangle.Empty
            };
        }

        public void Draw(SpriteBatch painter)
        {
            sprite.Draw(painter);
        }
    }
}