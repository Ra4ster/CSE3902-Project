using Microsoft.Xna.Framework;
using Byte.Sprite.Enemy;
using Byte.Block;
using Byte.Collidable;
using Byte.Item;

namespace Byte.Rooms
{
    /// <summary>
    /// Interface representing a room, including its collidables such as walls, doors, enemies, blocks, items, etc.
    /// </summary>
    public interface IRoom
    {
        IReadOnlyList<AbstractEnemy> Enemies { get; }
        IReadOnlyList<IBlock> Blocks { get; }
        IReadOnlyList<IItem> Items { get; }
        IReadOnlyList<Door> Doors { get; }
        IReadOnlyList<DoorWall> DoorWalls { get; }
        string Name { get; }
        string Background { get; }

        Rectangle Bounds { get; }
        Vector2 PlayerSpawnPosition { get; }

        IEnumerable<ICollidable> Collidables { get; }

        /// <summary>
        /// Updates all aspects of a room.
        /// </summary>
        /// <param name="gametime">Current time</param>
        void Update(GameTime gametime);

    }
}