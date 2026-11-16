
using Byte.Core.Rooms;
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

        private static readonly Point[] SearchDirections = [new(0, -1), new(1, 0), new(0, 1), new(-1, 0)];

        private readonly Dictionary<string, LowerMapRoom> mapRooms;
        private readonly MapPlayer mapPlayer;

        public LowerMap(LevelRepresentation rep, Texture2D pixel, Rectangle canvas)
        {
            RoomConnection[] connections = rep.ConnectionsOf(rep.StartingRoomName);

            Dictionary<string, Point> positions = BuildRoomPositions(
                rep.StartingRoomName,
                connections,
                out Point minBounds,
                out Point maxBounds
            );

            int layoutWidth = (maxBounds.X - minBounds.X) * HorizontalStep + RoomWidth;
            int layoutHeight = (maxBounds.Y - minBounds.Y) * VerticalStep + RoomHeight;

            if (layoutWidth > canvas.Width || layoutHeight > canvas.Height)
                throw new ArgumentOutOfRangeException(nameof(canvas), "Canvas is too small to display the rooms.");

            int left = canvas.X + (canvas.Width - layoutWidth) / 2;
            int top = canvas.Y + (canvas.Height - layoutHeight) / 2;

            mapRooms = new Dictionary<string, LowerMapRoom>(positions.Count, StringComparer.Ordinal);
            foreach (KeyValuePair<string, Point> pair in positions)
            {
                Rectangle placement = new(
                    left + (pair.Value.X - minBounds.X) * HorizontalStep,
                    top + (pair.Value.Y - minBounds.Y) * VerticalStep,
                    RoomWidth,
                    RoomHeight
                );

                mapRooms.Add(pair.Key, new LowerMapRoom(placement, pixel));
            }

            Rectangle startingRoom = mapRooms[rep.StartingRoomName].Bounds;
            int playerSize = RoomHeight;
            const int center = 2;

            mapPlayer = new MapPlayer(
                new Rectangle(
                    startingRoom.Center.X - playerSize / center,
                    startingRoom.Center.Y - playerSize / center,
                    playerSize,
                    playerSize
                ),
                pixel);
        }

        private static Dictionary<string, Point> BuildRoomPositions(string startingRoom, RoomConnection[] connections, out Point min, out Point max)
        {
            Dictionary<string, Point> positions = new(connections.Length + 1, StringComparer.Ordinal)
            {
                [startingRoom] = Point.Zero
            };

            HashSet<Point> occupied = [Point.Zero];
            min = Point.Zero;
            max = Point.Zero;

            foreach (RoomConnection connection in connections)
            {
                if (positions.ContainsKey(connection.TargetRoom))
                    continue;

                Point desiredPosition = DirectionOffset(connection.Direction);
                Point targetPosition = FindFreePosition(desiredPosition, occupied);

                positions.Add(connection.TargetRoom, targetPosition);
                _ = occupied.Add(targetPosition);

                if (targetPosition.X < min.X) min.X = targetPosition.X;
                if (targetPosition.Y < min.Y) min.Y = targetPosition.Y;
                if (targetPosition.X > max.X) max.X = targetPosition.X;
                if (targetPosition.Y > max.Y) max.Y = targetPosition.Y;
            }

            return positions;
        }

        public void Draw(SpriteBatch painter, Vector2 offset = default, bool showRooms = true)
        {
            if (showRooms)
            {
                foreach (LowerMapRoom room in mapRooms.Values)
                    room.Draw(painter, offset);
            }

            mapPlayer.Draw(painter, offset);
        }

        private static Point FindFreePosition(Point desired, HashSet<Point> occupied)
        {
            if (!occupied.Contains(desired))
                return desired;

            Queue<Point> candidates = new();
            HashSet<Point> visited = [desired]; // is occupied
            candidates.Enqueue(desired);

            while (candidates.TryDequeue(out Point candidate))
            {
                if (!occupied.Contains(candidate))
                    return candidate;

                foreach (Point direction in SearchDirections)
                {
                    Point next = candidate + direction;
                    if (visited.Add(next))
                        candidates.Enqueue(next);
                }
            }

            throw new InvalidOperationException("Could not place room on map.");
        }

        private static Point DirectionOffset(DoorDirection dir) => dir switch
        {
            DoorDirection.North => new Point(0, -1),
            DoorDirection.South => new Point(0, 1),
            DoorDirection.East => new Point(1, 0),
            DoorDirection.West => new Point(-1, 0),
            _ => Point.Zero
        };
    }
}