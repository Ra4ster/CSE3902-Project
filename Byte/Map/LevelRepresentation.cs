
using Byte.Rooms;

namespace Byte.Map
{
    public sealed class LevelRepresentation
    {
        /// <summary>
        /// Adjacency list: stores a room name and a list of connected room names.
        /// </summary>
        private Dictionary<string, string[]> adj;
        public LevelRepresentation(RoomManager manager)
        {
            adj = new Dictionary<string, string[]>(manager.Rooms.Count);
            foreach (IRoom room in manager.Rooms)
            {
                string[] edges = new string[room.Doors.Count];
                for (int i = 0; i < room.Doors.Count; i++)
                    edges[i] = room.Doors[i].TargetRoom;

                adj.Add(room.Name, edges);
            }
        }

        /// <summary>
        /// Returns the conections of a room based on its name, if connections exist.
        /// </summary>
        /// <param name="roomName">Name of the room whose edges you want</param>
        /// <returns>Either a list of adjacent room names, or null if none are found</returns>
        public string[]? ConnectionsOf(string roomName) => (adj.ContainsKey(roomName)) ? adj[roomName] : null;
    }
}