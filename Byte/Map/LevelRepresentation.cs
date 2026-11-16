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

            Dictionary<string, List<RoomConnection>> connections = new(StringComparer.OrdinalIgnoreCase);

            foreach (IRoom room in manager.Rooms)
            {
                connections[room.Name] = room.Doors
                    .Select(door => new RoomConnection(door.TargetRoom, door.doorDirection))
                    .ToList();
            }

            foreach ((string roomName, List<RoomConnection> roomConnections) in connections.ToArray())
            {
                foreach (RoomConnection connection in roomConnections.ToArray())
                {
                    if (string.IsNullOrEmpty(connection.TargetRoom))
                        continue;

                    RoomConnection reverse = new(roomName, Opposite(connection.Direction));

                    if (connections.TryGetValue(connection.TargetRoom, out List<RoomConnection>? targetConnections))
                    {
                        if (!targetConnections.Any(c => c.TargetRoom.Equals(roomName, StringComparison.OrdinalIgnoreCase)))
                        {
                            targetConnections.Add(reverse);
                        }
                    }
                }
            }

            adj = connections.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.ToArray(),
                StringComparer.OrdinalIgnoreCase);
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