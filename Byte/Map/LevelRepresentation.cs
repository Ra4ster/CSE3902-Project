using Byte.Core.Rooms;
using Byte.Rooms;

namespace Byte.Map
{
    public readonly record struct RoomConnection(string TargetRoom, DoorDirection Direction);

    public sealed class LevelRepresentation
    {
        private readonly Dictionary<string, RoomConnection[]> adj;

        public string StartingRoomName { get; }

        public LevelRepresentation(RoomManager manager)
        {
            ArgumentNullException.ThrowIfNull(manager);

            StartingRoomName = manager.CurrentRoom.Name;
            Dictionary<string, List<RoomConnection>> connections = manager.Rooms.ToDictionary(
                room => room.Name,
                room => room.Doors
                    .Select(door => new RoomConnection(door.TargetRoom, door.doorDirection))
                    .ToList(),
                StringComparer.Ordinal);

            foreach ((string roomName, List<RoomConnection> roomConnections) in connections.ToArray())
            {
                foreach (RoomConnection connection in roomConnections.ToArray())
                {
                    RoomConnection reverse = new(roomName, Opposite(connection.Direction));
                    List<RoomConnection> targetConnections = connections[connection.TargetRoom];
                    if (!targetConnections.Contains(reverse))
                        targetConnections.Add(reverse);
                }
            }

            adj = connections.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.ToArray(),
                StringComparer.Ordinal);
        }

        public RoomConnection[] ConnectionsOf(string roomName) =>
            adj.TryGetValue(roomName, out RoomConnection[]? connections) ? connections : [];

        public string[] RoomNames() => adj.Keys.ToArray();

        private static DoorDirection Opposite(DoorDirection direction) => direction switch
        {
            DoorDirection.North => DoorDirection.South,
            DoorDirection.South => DoorDirection.North,
            DoorDirection.East => DoorDirection.West,
            DoorDirection.West => DoorDirection.East,
            _ => DoorDirection.Default
        };
    }
}
