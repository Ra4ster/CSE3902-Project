using Microsoft.Xna.Framework;

namespace Byte.Factory
{
    public sealed class EnemyFactoryData
    {
        public required string Type { get; init; }
        public required Vector2 Position { get; init; }
        public Vector2[]? Path { get; init; }
        public float? Speed { get; init; }
        public float? FrameDuration { get; init; }
        public float? WaitDuration { get; init; }
        public float? Scale { get; init; }
    }

    public sealed class ItemFactoryData
    {
        public required string Type { get; init; }
        public required Vector2 Position { get; init; }
    }
}
