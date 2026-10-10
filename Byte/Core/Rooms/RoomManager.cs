using Byte.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Rooms
{
    public class RoomManager
    {
        public IReadOnlyList<IRoom> rooms { get; }

        public RoomManager(RoomData[] roomsData, EnemyFactory enemyFactory, BlockFactory blockFactory, ItemFactory itemFactory)
        {
            List<IRoom> tmpRooms = new List<IRoom>(roomsData.Count());
            foreach (RoomData roomData in roomsData)
            {
                IRoom room = new Room(roomData, enemyFactory, blockFactory, itemFactory);
                tmpRooms.Add(room);
            }

            rooms = tmpRooms;
        }

        public void Update(GameTime gametime)
        {
            foreach (IRoom room in rooms)
                room.Update(gametime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IRoom room in rooms)
                room.Draw(spriteBatch);
        }

        public void GoToRoom(int id)
        {
            if (id < 0 || id >= rooms.Count)
                throw new IndexOutOfRangeException($"Cannot go to a room of id: {id}");
            throw new NotImplementedException();
        }
    }
}