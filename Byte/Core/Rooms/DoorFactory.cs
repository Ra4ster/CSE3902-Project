using Byte.Rooms;
using Byte.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Byte.Core.Rooms
{
    internal class DoorFactory
    {
        private readonly Texture2D blockSheet;
        private const float scale = 5f;

        public DoorFactory(Texture2D blockSheet)
        {
            this.blockSheet = blockSheet;
        }

        public Door CreateDoor(DoorData data)
        {
            Vector2 position = RoomGrid.ToWorld(data.Position);

            DoorDirection direction = Enum.TryParse<DoorDirection>(data.Direction, true, out var parsedDir)
                ? parsedDir
                : DoorDirection.Default;

            Door door = new Door(data);
            (StaticSprite openSprite, StaticSprite closedSprite) = CreateDoorSprites(direction, position);

            door.SetSprites(openSprite, closedSprite);

            return door;
        }

        private (StaticSprite open, StaticSprite closed) CreateDoorSprites(DoorDirection direction, Vector2 position)
        {
            switch (direction) { 
            case DoorDirection.North:
                CreateNorthDoor(position);
                break;
            case DoorDirection.East:
                CreateEastDoor(position);
                break;
            case DoorDirection.South:
                CreateSouthDoor(position);
                break;
            case DoorDirection.West:
                CreateWestDoor(position);
                break;
            }
            // If no direction is specified, just create a north door. 
            return CreateNorthDoor(position);

         }
        private (StaticSprite open, StaticSprite closed) CreateNorthDoor(Vector2 position)
        {
            Rectangle openFrame = new Rectangle(848, 11, 32, 32);
            Rectangle closedFrame = new Rectangle(881, 11, 32, 32);

            return CreateOpenClosedSprites(position, openFrame, closedFrame);
        }

        private (StaticSprite open, StaticSprite closed) CreateSouthDoor(Vector2 position)
        {
            Rectangle openFrame = new Rectangle(848, 44, 32, 32);
            Rectangle closedFrame = new Rectangle(881, 44, 32, 32);

            return CreateOpenClosedSprites(position, openFrame, closedFrame);
        }

        private (StaticSprite open, StaticSprite closed) CreateEastDoor(Vector2 position)
        {
            Rectangle openFrame = new Rectangle(848, 77, 32, 32);
            Rectangle closedFrame = new Rectangle(881, 77, 32, 32);


            return CreateOpenClosedSprites(position, openFrame, closedFrame);
        }

        private (StaticSprite open, StaticSprite closed) CreateWestDoor(Vector2 position)
        {
            Rectangle openFrame = new Rectangle(848, 110, 32, 32);
            Rectangle closedFrame = new Rectangle(881, 110, 32, 32);

            return CreateOpenClosedSprites(position, openFrame, closedFrame);
        }

        private (StaticSprite open, StaticSprite closed) CreateOpenClosedSprites(Vector2 position, Rectangle openFrame, Rectangle closedFrame)
        {
            StaticSprite openSprite = new StaticSprite(
                blockSheet,
                position,
                Color.White,
                openFrame,
                scale
            );

            StaticSprite closedSprite = new StaticSprite(
                blockSheet,
                position,
                Color.White,
                closedFrame,
                scale
            );

            return (openSprite, closedSprite);
        }
    }
}