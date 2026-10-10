using Microsoft.Xna.Framework;

namespace Byte.Rooms
{
    internal class RoomManager
    {
        private static RoomManager instance = new RoomManager();

        private IReadOnlyList<IRoom>? rooms;

        private RoomManager() { }

        public static RoomManager Instance => instance;

        void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }

        void GoToRoom(int id)
        {
            if (id < 0 || id >= rooms.Count)
                throw new IndexOutOfRangeException($"Cannot go to a room of id: {id}");
            throw new NotImplementedException();
        }
    }
}