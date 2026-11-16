using Byte.Core.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Map
{
    public sealed class UpperMap
    {
        private const int NativeTileSize = 8;
        private const int RoomSize = (int)(NativeTileSize * GameConstants.SCALE);

        private static readonly Point RoomTilesStart = new(519, 108);
        private const int TileStride = 9;
        private static readonly Rectangle MapBackgroundSource = new(386, 120, 64, 64);

        [Flags]
        private enum DoorMask
        {
            None = 0,
            East = 1 << 0,
            West = 1 << 1,
            South = 1 << 2,
            North = 1 << 3
        }

        private static readonly int[] MaskToTileIndex =
        [
            0,  // 0000: None
            1,  // 0001: East
            2,  // 0010: West
            3,  // 0011: East + West
            4,  // 0100: South
            5,  // 0101: South + East
            6,  // 0110: South + West
            7,  // 0111: South + East + West
            8,  // 1000: North
            9,  // 1001: North + East
            10, // 1010: North + West
            11, // 1011: North + East + West
            12, // 1100: North + South
            13, // 1101: North + South + East
            14, // 1110: North + South + West
            15  // 1111: All 4 doors
        ];

        private static readonly Point[] SearchDirections = [new(0, -1), new(1, 0), new(0, 1), new(-1, 0)];

        private readonly Dictionary<string, UpperMapRoom> mapRooms;
        private readonly HashSet<string> visitedRooms;
        private readonly Texture2D spriteSheet;
        private readonly Texture2D pixel;
        private readonly Rectangle backgroundRect;

        private readonly MapPlayer mapPlayer;
        private string currentRoomName;

        public UpperMap(LevelRepresentation rep, Texture2D spriteSheet, Rectangle canvas)
        {
            this.spriteSheet = spriteSheet;

            pixel = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            backgroundRect = canvas;
            currentRoomName = rep.StartingRoomName;
            mapPlayer = new MapPlayer(pixel);

            Dictionary<string, Point> positions = BuildRoomPositions(
                rep,
                out Point minBounds,
                out Point maxBounds
            );

            int layoutWidth = (maxBounds.X - minBounds.X + 1) * RoomSize;
            int layoutHeight = (maxBounds.Y - minBounds.Y + 1) * RoomSize;

            if (layoutWidth > canvas.Width || layoutHeight > canvas.Height)
                throw new ArgumentOutOfRangeException(nameof(canvas), "Canvas is too small to display the rooms.");

            int left = canvas.X + (canvas.Width - layoutWidth) / 2;
            int top = canvas.Y + (canvas.Height - layoutHeight) / 2;

            mapRooms = new Dictionary<string, UpperMapRoom>(positions.Count, StringComparer.OrdinalIgnoreCase);
            visitedRooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { rep.StartingRoomName };

            foreach (KeyValuePair<string, Point> pair in positions)
            {
                string roomName = pair.Key;
                Point gridPos = pair.Value;

                Rectangle placement = new(
                    left + (gridPos.X - minBounds.X) * RoomSize,
                    top + (gridPos.Y - minBounds.Y) * RoomSize,
                    RoomSize,
                    RoomSize
                );

                DoorMask mask = ComputeDoorMask(rep.ConnectionsOf(roomName));
                int tileIndex = MaskToTileIndex[(int)mask];

                // Fixed typo: added '*' for TileStride multiplication
                Rectangle sourceRect = new(
                    RoomTilesStart.X + (tileIndex * TileStride),
                    RoomTilesStart.Y,
                    NativeTileSize,
                    NativeTileSize
                );

                mapRooms.Add(roomName, new UpperMapRoom(placement, sourceRect, spriteSheet));
            }
        }

        public void OnRoomEntered(string roomName)
        {
            if (!mapRooms.ContainsKey(roomName))
                return;

            visitedRooms.Add(roomName);
            currentRoomName = roomName;
        }

        public void Draw(SpriteBatch painter, Vector2 offset = default)
        {
            Rectangle shiftedBg = new(
                backgroundRect.X + (int)offset.X,
                backgroundRect.Y + (int)offset.Y,
                backgroundRect.Width,
                backgroundRect.Height);

            painter.Draw(spriteSheet, shiftedBg, MapBackgroundSource, Color.White);

            // Dynamically draw only the rooms that have been visited
            foreach (string roomName in visitedRooms)
            {
                if (mapRooms.TryGetValue(roomName, out UpperMapRoom? room))
                    room.Draw(painter, offset);
            }

            // Draw player on top of the current room bounds using the new signature
            if (mapRooms.TryGetValue(currentRoomName, out UpperMapRoom? currentRoom))
            {
                mapPlayer.Draw(painter, currentRoom.Bounds, offset);
            }
        }

        private static DoorMask ComputeDoorMask(RoomConnection[] connections)
        {
            DoorMask mask = DoorMask.None;
            foreach (RoomConnection conn in connections)
            {
                mask |= conn.Direction switch
                {
                    DoorDirection.North => DoorMask.North,
                    DoorDirection.South => DoorMask.South,
                    DoorDirection.East => DoorMask.East,
                    DoorDirection.West => DoorMask.West,
                    _ => DoorMask.None
                };
            }
            return mask;
        }

        private static Dictionary<string, Point> BuildRoomPositions(LevelRepresentation rep, out Point min, out Point max)
        {
            Dictionary<string, Point> positions = new(StringComparer.OrdinalIgnoreCase)
            {
                [rep.StartingRoomName] = Point.Zero
            };

            HashSet<Point> occupied = [Point.Zero];
            Queue<string> frontier = new();
            frontier.Enqueue(rep.StartingRoomName);

            min = Point.Zero;
            max = Point.Zero;

            foreach (string roomName in rep.RoomNames())
            {
                if (!positions.ContainsKey(roomName))
                {
                    frontier.Enqueue(roomName);
                }
            }

            while (frontier.TryDequeue(out string? currentRoom))
            {
                Point currentPos = positions.TryGetValue(currentRoom, out Point p) ? p : Point.Zero;

                foreach (RoomConnection connection in rep.ConnectionsOf(currentRoom))
                {
                    if (positions.ContainsKey(connection.TargetRoom))
                        continue;

                    Point desiredPosition = currentPos + DirectionOffset(connection.Direction);
                    Point targetPosition = FindFreePosition(desiredPosition, occupied);

                    positions.Add(connection.TargetRoom, targetPosition);
                    occupied.Add(targetPosition);
                    frontier.Enqueue(connection.TargetRoom);

                    if (targetPosition.X < min.X) min.X = targetPosition.X;
                    if (targetPosition.Y < min.Y) min.Y = targetPosition.Y;
                    if (targetPosition.X > max.X) max.X = targetPosition.X;
                    if (targetPosition.Y > max.Y) max.Y = targetPosition.Y;
                }
            }

            return positions;
        }

        private static Point FindFreePosition(Point desired, HashSet<Point> occupied)
        {
            if (!occupied.Contains(desired))
                return desired;

            Queue<Point> candidates = new();
            HashSet<Point> visited = [desired];
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