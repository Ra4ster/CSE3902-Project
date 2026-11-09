using Byte.Core.Rooms;

namespace Byte.Rooms
{
    public readonly record struct GridPosition(int X, int Y);

    public sealed class RoomData
    {
        /// <summary>
        /// Name of room to address
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Background of room (see Background.cs)
        /// </summary>
        public required string Background { get; set; }
        /// <summary>
        /// Player spawn position in room; likely not used if the room is only accessible by doors.
        /// </summary>
        public required GridPosition PlayerSpawn { get; set; }
        /// <summary>
        /// Only one json may have this flag, signifying where to begin.
        /// </summary>
        public bool IsStartingRoom { get; set; }
        public required int Width { get; set; }
        public required int Height { get; set; }

        /// <summary>
        /// Stores a dictionary of every block's location.
        /// </summary>
        public Dictionary<string, List<GridPosition>> Tiles { get; set; } = [];
        public IList<EnemyData> Enemies { get; set; } = [];
        public IList<ItemData> Items { get; set; } = [];
        public IList<DoorData> Doors { get; set; } = [];
    }

    public class GameObjectData
    {
        /// <summary>
        /// Type of object
        /// </summary>
        public required string Type { get; set; }
        /// <summary>
        /// Position in room (must be within room size)
        /// </summary>
        public required GridPosition Position { get; set; }
    }

    public sealed class DoorData
    {
        /// <summary>
        /// Type (i.e. locked, sealed, blown, wall, etc.)
        /// </summary>
        public DoorState Type { get; set; } = DoorState.Open;
        /// <summary>
        /// Position on the room grid (must be within room size)
        /// </summary>
        public required GridPosition Position { get; set; }
        /// <summary>
        /// North, South, East, or West
        /// </summary>
        public required string Direction { get; set; }
        /// <summary>
        /// Room name to use for exit
        /// </summary>
        public required string TargetRoom { get; set; }
        /// <summary>
        /// Position in target room to arrive at; useful for if there is something blocking the door.
        /// </summary>
        public GridPosition? ArrivalPosition { get; set; }
    }

    public sealed class EnemyData : GameObjectData
    {
        /// <summary>
        /// Path (optional) for enemy to take.
        /// </summary>
        public IList<GridPosition>? Path { get; set; }
        /// <summary>
        /// Speed of enemy movement.
        /// </summary>
        public float? Speed { get; set; }
        /// <summary>
        /// Animation speed (how long the frames last)
        /// </summary>
        public float? FrameDuration { get; set; }
        /// <summary>
        /// How long a waiting enemy would wait after a checkpoint.
        /// </summary>
        public float? WaitDuration { get; set; }
        /// <summary>
        /// Scale of an enemy (enables changing each enemy size!)
        /// </summary>
        public float? Scale { get; set; }
    }

    public sealed class ItemData : GameObjectData
    { }
}