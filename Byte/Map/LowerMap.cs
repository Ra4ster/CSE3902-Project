
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Map
{
    public sealed class LowerMap
    {
        private const int RoomWidth = (int)(10 * GameConstants.SCALE);
        private const int RoomHeight = (int)(4 * GameConstants.SCALE);
        private const int RoomGap = (int)(2 * GameConstants.SCALE);
        private const int HorizontalStep = RoomWidth + RoomGap;
        private const int VerticalStep = RoomHeight + RoomGap;

        private readonly Dictionary<string, LowerMapRoom> mapRooms;
        private readonly MapPlayer mapPlayer;

        public LowerMap(LevelRepresentation representation, Texture2D pixel, Rectangle canvas)
        {
            ArgumentNullException.ThrowIfNull(representation);
            ArgumentNullException.ThrowIfNull(pixel);

            RoomConnection[] connections = representation.ConnectionsOf(representation.StartingRoomName);
            Dictionary<string, Point> positions = BuildRoomPositions(representation.StartingRoomName, connections);
            mapRooms = new Dictionary<string, LowerMapRoom>(positions.Count);

            Point[] bounds = positions.Values
                .Concat(connections.Select(connection => DirectionOffset(connection.Direction)))
                .ToArray();
            int minX = bounds.Min(position => position.X);
            int maxX = bounds.Max(position => position.X);
            int minY = bounds.Min(position => position.Y);
            int maxY = bounds.Max(position => position.Y);
            int layoutWidth = (maxX - minX) * HorizontalStep + RoomWidth;
            int layoutHeight = (maxY - minY) * VerticalStep + RoomHeight;
            if (layoutWidth > canvas.Width || layoutHeight > canvas.Height)
                throw new ArgumentOutOfRangeException(nameof(canvas), "Canvas is too small to display the rooms.");

            int left = canvas.X + (canvas.Width - layoutWidth) / 2;
            int top = canvas.Y + (canvas.Height - layoutHeight) / 2;
            foreach ((string name, Point position) in positions)
            {
                Rectangle placement = new(
                    left + (position.X - minX) * HorizontalStep,
                    top + (position.Y - minY) * VerticalStep,
                    RoomWidth,
                    RoomHeight);
                mapRooms.Add(name, new LowerMapRoom(placement, pixel));
            }

            Rectangle startingRoom = mapRooms[representation.StartingRoomName].Bounds;
            int playerSize = RoomHeight;
            mapPlayer = new MapPlayer(
                new Rectangle(
                    startingRoom.Center.X - playerSize / 2,
                    startingRoom.Center.Y - playerSize / 2,
                    playerSize,
                    playerSize),
                pixel);
        }

        public void Draw(SpriteBatch painter)
        {
            Draw(painter, Vector2.Zero);
        }

        public void Draw(SpriteBatch painter, Vector2 offset)
        {
            ArgumentNullException.ThrowIfNull(painter);
            foreach (LowerMapRoom room in mapRooms.Values)
                room.Draw(painter, offset);
            mapPlayer.Draw(painter, offset);
        }

        private static Dictionary<string, Point> BuildRoomPositions(string startingRoom, RoomConnection[] connections)
        {
            Dictionary<string, Point> positions = new(StringComparer.Ordinal)
            {
                [startingRoom] = Point.Zero
            };
            HashSet<Point> occupied = [];
            _ = occupied.Add(Point.Zero);
            foreach (RoomConnection connection in connections)
            {
                if (positions.ContainsKey(connection.TargetRoom))
                    continue;

                Point desiredPosition = DirectionOffset(connection.Direction);
                Point targetPosition = FindFreePosition(desiredPosition, occupied);
                positions.Add(connection.TargetRoom, targetPosition);
                _ = occupied.Add(targetPosition);
            }

            return positions;
        }

        private static Point FindFreePosition(Point desired, HashSet<Point> occupied)
        {
            Queue<Point> candidates = [];
            HashSet<Point> visited = [desired];
            candidates.Enqueue(desired);
            Point[] directions = [new(0, -1), new(1, 0), new(0, 1), new(-1, 0)];

            while (candidates.TryDequeue(out Point candidate))
            {
                if (!occupied.Contains(candidate))
                    return candidate;

                foreach (Point direction in directions)
                {
                    Point next = candidate + direction;
                    if (visited.Add(next))
                        candidates.Enqueue(next);
                }
            }

            throw new InvalidOperationException("Could not place a room on the map.");
        }

        private static Point DirectionOffset(Byte.Core.Rooms.DoorDirection direction) => direction switch
        {
            Byte.Core.Rooms.DoorDirection.North => new Point(0, -1),
            Byte.Core.Rooms.DoorDirection.South => new Point(0, 1),
            Byte.Core.Rooms.DoorDirection.East => new Point(1, 0),
            Byte.Core.Rooms.DoorDirection.West => new Point(-1, 0),
            _ => Point.Zero
        };
    }
}