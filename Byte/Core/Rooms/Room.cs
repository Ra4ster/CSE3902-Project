
using Byte.Block;
using Byte.Factory;
using Byte.Item;
using Byte.Sprite;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Rooms
{
    internal class Room : IRoom
    {
        private readonly List<AbstractEnemy> enemies = [];
        private readonly List<IBlock> blocks = [];
        private readonly List<IItem> items = [];

        public IReadOnlyList<AbstractEnemy> Enemies => enemies;
        public IReadOnlyList<IBlock> Blocks => blocks;
        public IReadOnlyList<IItem> Items => items;

        public string name;

        public Room(RoomData data,
        IFactory<EnemyData, AbstractEnemy> enemyFactory,
        IFactory<BlockData, IBlock> blockFactory,
        IFactory<ItemData, IItem> itemFactory)
        {
            name = data.Name;
            foreach (EnemyData enemyData in data.Enemies)
            enemies.Add(enemyFactory.Create(enemyData));
            foreach (BlockData blockData in data.Blocks)
            blocks.Add(blockFactory.Create(blockData));
            foreach (ItemData itemData in data.Items)
            items.Add(itemFactory.Create(itemData));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AbstractEnemy enemy in enemies)
                enemy.Draw(spriteBatch);
            foreach (IBlock block in blocks)
                block.Draw(spriteBatch);
            foreach (IItem item in items)
                item.Draw(spriteBatch);
        }

        public void Update(GameTime gametime)
        {
            foreach (AbstractEnemy enemy in enemies)
                enemy.Update(gametime);
            foreach (IBlock block in blocks)
                block.Update(gametime);
            foreach (IItem item in items)
                item.Update(gametime);
        }
    }
}