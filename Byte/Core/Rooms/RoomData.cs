namespace Byte.Rooms
{
    public readonly record struct GridPosition(int X, int Y);

    public sealed class RoomData
    {
        public required string Name { get; set; }
        public required string Background { get; set; }
        public required GridPosition PlayerSpawn { get; set; }
        public bool IsStartingRoom { get; set; }
        public required int Width { get; set; }
        public required int Height { get; set; }

        public Dictionary<string, List<GridPosition>> Tiles { get; set; } = [];
        public List<EnemyData> Enemies { get; set; } = [];
        public List<ItemData> Items { get; set; } = [];
        public List<DoorData> Doors { get; set; } = [];
    }

    public class GameObjectData
    {
        public required string Type { get; set; }
        public required GridPosition Position { get; set; }
    }

    // May change in the future?
    public sealed class DoorData
    {
        public required GridPosition Position { get; set; }
        public required string Direction { get; set; }
        public required string TargetRoom { get; set; }
        public GridPosition? ArrivalPosition { get; set; }
        public required string doorState { get; set; }
    }

    public sealed class EnemyData : GameObjectData
    {
        public GridPosition[]? Path { get; set; }
        public float? Speed { get; set; }
        public float? FrameDuration { get; set; }
        public float? WaitDuration { get; set; }
        public float? Scale { get; set; }
    }

    public sealed class ItemData : GameObjectData
    { }
}