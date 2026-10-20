using Byte.Collidable;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Byte.Rooms
{
    public enum DoorDirection
    {
        North,
        South,
        East,
        West,
        Default
    }
    public sealed class Door : ICollidable
    {
        public DoorDirection Direction { get; }
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
                Direction = parsedDir;
            }
            else
            {
                Direction = DoorDirection.Default;
            }

            Vector2 topLeft = RoomGrid.ToWorld(data.Position);
            int side = (int)RoomGrid.TILE_WORLD_SIZE;
            BoundingBox = new Rectangle((int)topLeft.X, (int)topLeft.Y, side, side);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            StaticSprite? spriteToDraw = IsOpen ? openSprite : closedSprite;

            if (spriteToDraw != null)
            {
                spriteToDraw.Draw(spriteBatch);
            }
        }
    }
}