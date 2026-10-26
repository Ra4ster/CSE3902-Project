
using Byte.Block;
using Byte.Collidable;
using Byte.Core.Rooms;
using Byte.Factory;
using Byte.Item;
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
        private readonly List<Door> doors = [];

        public IReadOnlyList<AbstractEnemy> Enemies => enemies;
        public IReadOnlyList<IBlock> Blocks => blocks;
        public IReadOnlyList<IItem> Items => items;
        public IReadOnlyList<Door> Doors => doors;
        public string Name { get; }
        public Vector2 PlayerSpawnPosition { get; }
        public bool IsStartingRoom { get; }

        public IEnumerable<ICollidable> Collidables =>
            blocks.OfType<ICollidable>()
                .Concat(enemies)
                .Concat(items)
                .Concat(doors);

        public Room(RoomData data,
        IFactory<EnemyFactoryData, AbstractEnemy> enemyFactory,
        BlockFactory blockFactory,
        IFactory<ItemFactoryData, IItem> itemFactory)
        {
            Name = data.Name;
            PlayerSpawnPosition = RoomGrid.ToWorld(data.PlayerSpawn);
            IsStartingRoom = data.IsStartingRoom;

            foreach ((string tileType, List<GridPosition> positions) in data.Tiles)
                foreach (GridPosition position in positions)
                    blocks.Add(blockFactory.Create(tileType, RoomGrid.ToWorld(position)));

            foreach (EnemyData enemyData in data.Enemies)
            {
                EnemyFactoryData worldData = new EnemyFactoryData
                {
                    Type = enemyData.Type,
                    Position = RoomGrid.ToWorld(enemyData.Position),
                    Path = enemyData.Path?.Select(RoomGrid.ToWorld).ToArray(),
                    Speed = enemyData.Speed,
                    FrameDuration = enemyData.FrameDuration,
                    WaitDuration = enemyData.WaitDuration,
                    Scale = enemyData.Scale
                };
                enemies.Add(enemyFactory.Create(worldData));
            }

            foreach (ItemData itemData in data.Items)
            {
                ItemFactoryData worldData = new ItemFactoryData
                {
                    Type = itemData.Type,
                    Position = RoomGrid.ToWorld(itemData.Position)
                };
                items.Add(itemFactory.Create(worldData));
            }
            DoorFactory doorFactory = new DoorFactory(GameAssets.Instance.BlockSheet);
            foreach (DoorData doorData in data.Doors)
                doors.Add(doorFactory.CreateDoor(doorData));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IBlock block in blocks)
                block.Draw(spriteBatch);
            foreach (IItem item in items)
                item.Draw(spriteBatch);
            foreach (AbstractEnemy enemy in enemies)
                enemy.Draw(spriteBatch);
            foreach (Door door in doors)
                door.Draw(spriteBatch);
        }

        public void Update(GameTime gametime)
        {
            foreach (AbstractEnemy enemy in enemies)
                enemy.Update(gametime);
            _ = enemies.RemoveAll(enemy => enemy.IsDefeated);
            foreach (IBlock block in blocks)
                block.Update(gametime);
            foreach (IItem item in items)
                item.Update(gametime);
            _ = items.RemoveAll(item => item.IsCollected);
        }
    }
}