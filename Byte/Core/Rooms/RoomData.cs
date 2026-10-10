using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Byte.Rooms
{
    internal sealed class RoomData
    {
        public required string Name { get; set; }
        public Vector2 WorldPosition { get; set; }
        public required string Background { get; set; }
        public required List<GameObjectData> Objects { get; set; }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
    [JsonDerivedType(typeof(EnemyData), "enemy")]
    public class GameObjectData
    {
        public required string Type { get; set; }
        public Vector2 Position { get; set; }
    }

    public sealed class EnemyData : GameObjectData
    {
        public Vector2[]? Path { get; set; }
        public float? Speed { get; set; }
        public float? FrameDuration { get; set; }
        public float? WaitDuration { get; set; }
        public float? Scale { get; set; }
    }
}