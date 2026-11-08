
using Byte.Block;
using Byte.Collidable;
using Byte.Core.Rooms;
using Byte.Factory;
using Byte.Item;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;

namespace Byte.Rooms
{
    internal class Room : IRoom
    {
        private readonly List<AbstractEnemy> enemies = [];
        private readonly List<IBlock> blocks = [];
        private readonly List<IItem> items = [];
        private readonly List<Door> doors = [];

        public Rectangle Bounds { get; }

        public IReadOnlyList<AbstractEnemy> Enemies => enemies;
        public IReadOnlyList<IBlock> Blocks => blocks;
        public IReadOnlyList<IItem> Items => items;
        public IReadOnlyList<Door> Doors => doors;
        public string Name { get; }
        public string Background { get; }
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
            Background = data.Background;
            PlayerSpawnPosition = RoomGrid.ToWorld(data.PlayerSpawn);
            IsStartingRoom = data.IsStartingRoom;
            Bounds = new Rectangle(0, 0,
            (int)(data.Width * RoomGrid.TILE_WORLD_SIZE),
            (int)(data.Height * RoomGrid.TILE_WORLD_SIZE));

            foreach ((string tileType, List<GridPosition> positions) in data.Tiles)
                foreach (GridPosition position in positions)
                {
                    IBlock block = blockFactory.Create(tileType, RoomGrid.ToWorld(position));
                    blocks.Add(block);
                }

            AddBoundary(data, blockFactory);

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
                AbstractEnemy enemy = enemyFactory.Create(worldData);
                enemy.Bounds = Bounds;
                enemies.Add(enemy);
            }

            foreach (ItemData itemData in data.Items)
            {
                ItemFactoryData worldData = new ItemFactoryData
                {
                    Type = itemData.Type,
                    Position = RoomGrid.ToWorld(itemData.Position)
                };
                IItem item = itemFactory.Create(worldData);
                items.Add(item);
            }
            DoorFactory doorFactory = new DoorFactory(GameAssets.Instance.BlockSheet);
            foreach (DoorData doorData in data.Doors)
            {
                Door door = doorFactory.CreateDoor(doorData);
                doors.Add(door);
            }
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

        private void AddBoundary(RoomData data, BlockFactory blockFactory)
        {
            void Add(float x, float y) =>
                blocks.Add(blockFactory.CreateBoundaryBlock(new Vector2(
                    x * RoomGrid.TILE_WORLD_SIZE,
                    y * RoomGrid.TILE_WORLD_SIZE)));

            for (int x = 0; x < data.Width; ++x)
            {
                if (!IsDoorOpening(data.Doors, x, -1, DoorDirection.North))
                    Add(x, -1);
                if (!IsDoorOpening(data.Doors, x, data.Height, DoorDirection.South))
                    Add(x, data.Height);
            }
            for (int y = 0; y < data.Height; ++y)
            {
                float shiftedY = y - 0.5f;
                if (!IsDoorOpening(data.Doors, -1, shiftedY, DoorDirection.West))
                    Add(-1, shiftedY);
                if (!IsDoorOpening(data.Doors, data.Width, shiftedY, DoorDirection.East))
                    Add(data.Width, shiftedY);
            }
        }

        private static bool IsDoorOpening(
            IEnumerable<DoorData> doors,
            float boundaryX,
            float boundaryY,
            DoorDirection direction)
        {
            foreach (DoorData door in doors)
            {
                if (!Enum.TryParse(door.Direction, true, out DoorDirection doorDirection) ||
                    doorDirection != direction)
                    continue;

                if (direction is DoorDirection.North or DoorDirection.South)
                {
                    if (boundaryX >= door.Position.X - 1 && boundaryX < door.Position.X + 1)
                        return true;
                }
                else if (boundaryY >= door.Position.Y - 0.5f &&
                    boundaryY < door.Position.Y + 1.5f)
                {
                    return true;
                }
            }

            return false;
        }
    }
}