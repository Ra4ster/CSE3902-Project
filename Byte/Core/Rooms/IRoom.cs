using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        string Name { get; }

        Rectangle Bounds { get; }
        Vector2 PlayerSpawnPosition { get; }

        IEnumerable<ICollidable> Collidables { get; }

        /// <summary>
        /// Updates all aspects of a room.
        /// </summary>
        /// <param name="gametime">Current time</param>
        void Update(GameTime gametime);

        /// <summary>
        /// Draws everyting inside a room
        /// </summary>
        /// <param name="spriteBatch">Painter for sprites</param>
        void Draw(SpriteBatch spriteBatch);
    }
}