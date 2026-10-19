using Byte.Collidable;
using Microsoft.Xna.Framework;

namespace Byte.Rooms
{
    /// <summary>
    /// One door tile. Program hooks up OnCollide to switch rooms.
    /// </summary>
    public sealed class Door : ICollidable
    {
        public DoorData Data { get; }
        public Rectangle BoundingBox { get; }
        public CollisionProperties Properties { get; } = new CollisionProperties
        {
            Type = CollidableTypes.Door,
            IsStatic = true
        };

        public string TargetRoom => Data.TargetRoom;

        public Door(DoorData data)
        {
            Data = data;
            Vector2 topLeft = RoomGrid.ToWorld(data.Position);
            int side = (int)RoomGrid.TILE_WORLD_SIZE;
            BoundingBox = new Rectangle((int)topLeft.X, (int)topLeft.Y, side, side);
        }
    }
}
