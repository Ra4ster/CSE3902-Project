using Byte.Factory;
using Byte.Item;
using Byte.Sprite.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Rooms
{
    public class RoomManager
    {
        private readonly Dictionary<string, Room> roomsByName = new(StringComparer.Ordinal);
        private readonly List<Room> rooms = [];
        public IReadOnlyList<IRoom> Rooms { get; }
        public IRoom CurrentRoom { get; private set; }

        public RoomManager(IEnumerable<RoomData> roomsData,
        IFactory<EnemyFactoryData, AbstractEnemy> enemyFactory,
        BlockFactory blockFactory,
        IFactory<ItemFactoryData, IItem> itemFactory)
        {
            RoomData[] roomDataArray = roomsData.ToArray();
            foreach (RoomData roomData in roomDataArray)
            {
                Room room = new Room(roomData, enemyFactory, blockFactory, itemFactory);
                if (!roomsByName.TryAdd(room.Name, room))
                    throw new InvalidDataException($"Duplicate room name '{room.Name}'.");
                rooms.Add(room);
            }

            if (rooms.Count == 0)
                throw new InvalidDataException("No rooms were provided.");

            Room[] startingRooms = rooms.Where(room => room.IsStartingRoom).ToArray();
            if (startingRooms.Length != 1)
                throw new InvalidDataException($"Expected exactly one starting room, found {startingRooms.Length}.");

            ValidateDoors(roomDataArray);
            Rooms = rooms.AsReadOnly();
            CurrentRoom = startingRooms[0];
        }

        public void Update(GameTime gameTime)
        {
            CurrentRoom.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentRoom.Draw(spriteBatch);
        }

        public Vector2 GoToRoom(string name)
        {
            if (!roomsByName.TryGetValue(name, out Room? room))
                throw new KeyNotFoundException($"Cannot go to unknown room '{name}'.");

            CurrentRoom = room;
            foreach (AbstractEnemy enemy in CurrentRoom.Enemies)
                if (!enemy.IsDefeated) enemy.position = enemy.startPos;
            return room.PlayerSpawnPosition;
        }

        public Vector2 NextRoom() => StepRoom(1);

        public Vector2 PreviousRoom() => StepRoom(-1);

        private Vector2 StepRoom(int offset)
        {
            int index = rooms.IndexOf((Room)CurrentRoom) + offset;
            if (index < 0)
                index = rooms.Count - 1;
            else if (index >= rooms.Count)
                index = 0;
            return GoToRoom(rooms[index].Name);
        }

        public Vector2 EnterDoor(Door door)
        {
            if (door == null) throw new ArgumentNullException("Door must exist");

            if (!CurrentRoom.Doors.Contains(door))
                throw new InvalidOperationException($"Door does not belong to current room '{CurrentRoom.Name}'.");
            if (!roomsByName.TryGetValue(door.TargetRoom, out Room? targetRoom))
                throw new InvalidDataException(
                    $"Door in room '{CurrentRoom.Name}' targets unknown room '{door.TargetRoom}'.");

            CurrentRoom = targetRoom;

            foreach (AbstractEnemy enemy in CurrentRoom.Enemies)
                if (!enemy.IsDefeated) enemy.position = enemy.startPos;

            return door.Data.ArrivalPosition is GridPosition arrivalPosition
                ? RoomGrid.ToWorld(arrivalPosition)
                : targetRoom.PlayerSpawnPosition;
        }

        private void ValidateDoors(IEnumerable<RoomData> roomsData)
        {
            Dictionary<string, RoomData> roomDataByName = roomsData.ToDictionary(data => data.Name, StringComparer.Ordinal);
            foreach (RoomData roomData in roomDataByName.Values)
            {
                foreach (DoorData door in roomData.Doors)
                {
                    if (!roomDataByName.TryGetValue(door.TargetRoom, out RoomData? targetRoom))
                        throw new InvalidDataException(
                            $"Door in room '{roomData.Name}' targets unknown room '{door.TargetRoom}'.");
                    if (door.ArrivalPosition is GridPosition position)
                        RoomLoader.ValidateGridPosition(position, targetRoom, $"arrival position for door in '{roomData.Name}'");
                }
            }
        }
    }
}