using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite.Enemy;
using Byte.Block;
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
        IReadOnlyList<DoorData> Doors { get; }
        string Name { get; }
        Vector2 PlayerSpawnPosition { get; }

        // TODO: IReadOnlyList<ICollidable> collidables { get; }

        /// <summary>
        /// Updates all aspects of a room.
        /// </summary>
        /// <param name="gametime">Current time</paramRoomManaIRoom
        void Update(GameTime gametime);

        /// <summary>
        /// Draws everyting inside a room
        /// </summary>
        /// <param name="spriteBatch">Painter for sprites</param>
        void Draw(SpriteBatch spriteBatch);
    }
}