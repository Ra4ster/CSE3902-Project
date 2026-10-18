using Byte.Rooms;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Byte
{
    internal class RoomLoader
    {

        public sealed class GridPositionJsonConverter : JsonConverter<GridPosition>
        {
            public override GridPosition Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options
            )
            {
                if (reader.TokenType != JsonTokenType.StartArray ||
                    !reader.Read() || reader.TokenType != JsonTokenType.Number ||
                    !reader.TryGetInt32(out int x) ||
                    !reader.Read() || reader.TokenType != JsonTokenType.Number ||
                    !reader.TryGetInt32(out int y) ||
                    !reader.Read() || reader.TokenType != JsonTokenType.EndArray)
                    throw new JsonException("A grid position must be an array of two integers: [x, y].");
                return new GridPosition(x, y);
            }

            public override void Write(
                Utf8JsonWriter writer,
                GridPosition value,
                JsonSerializerOptions options
            )
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(value.X);
                writer.WriteNumberValue(value.Y);
                writer.WriteEndArray();
            }

        }
        public static RoomData GetRoomData(string path)
        {
            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            using (FileStream jsonStream = new FileStream(fullPath, FileMode.Open))
            {
                JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                options.Converters.Add(new GridPositionJsonConverter());

                RoomData? data = JsonSerializer.Deserialize<RoomData>(jsonStream, options);
                if (data is null)
                    throw new InvalidDataException($"Room file {fullPath} contained no room data.");
                Validate(data, fullPath);
                return data;
            }
        }

        private static void Validate(RoomData data, string source)
        {
            if (string.IsNullOrWhiteSpace(data.Name))
                throw new InvalidDataException($"Room file {source} has no name.");
            if (string.IsNullOrWhiteSpace(data.Background))
                throw new InvalidDataException($"Room file {source} has no background.");
            if (data.Width <= 0 || data.Height <= 0)
                throw new InvalidDataException($"Room file {source} must have positive width and height.");
            if (data.Tiles is null || data.Enemies is null || data.Items is null || data.Doors is null)
                throw new InvalidDataException($"Room file {source} contains a null collection.");

            HashSet<(int X, int Y)> tilePositions = [];
            foreach ((string tileType, List<GridPosition> positions) in data.Tiles)
            {
                if (string.IsNullOrWhiteSpace(tileType))
                    throw new InvalidDataException($"Room file {source} contains a tile without a type.");
                if (positions is null)
                    throw new InvalidDataException($"Room file {source} has a null position list for tile '{tileType}'.");

                foreach (GridPosition position in positions)
                {
                    if (position.X < 0 || position.X >= data.Width ||
                        position.Y < 0 || position.Y >= data.Height)
                        throw new InvalidDataException(
                            $"Room file {source} has an invalid '{tileType}' tile position ({position.X}, {position.Y}).");
                    if (!tilePositions.Add((position.X, position.Y)))
                        throw new InvalidDataException(
                            $"Room file {source} contains multiple tiles at ({position.X}, {position.Y}).");
                }
            }

            ValidateGridPosition(data.PlayerSpawn, data, "player spawn", source);
            foreach (EnemyData enemy in data.Enemies)
            {
                ValidateGridPosition(enemy.Position, data, $"enemy '{enemy.Type}' spawn", source);
                if (enemy.Path is not null)
                    foreach (GridPosition position in enemy.Path)
                        ValidateGridPosition(position, data, $"enemy '{enemy.Type}' path point", source);
            }
            foreach (ItemData item in data.Items)
                ValidateGridPosition(item.Position, data, $"item '{item.Type}' spawn", source);
            foreach (DoorData door in data.Doors)
            {
                ValidateGridPosition(door.Position, data, "door position", source);
                if (string.IsNullOrWhiteSpace(door.TargetRoom))
                    throw new InvalidDataException($"Room file {source} contains a door without a target room.");
                if (door.Direction is not ("North" or "South" or "East" or "West"))
                    throw new InvalidDataException(
                        $"Room file {source} contains invalid door direction '{door.Direction}'.");
            }
        }

        internal static void ValidateGridPosition(GridPosition position, RoomData room, string description) =>
            ValidateGridPosition(position, room, description, room.Name);

        private static void ValidateGridPosition(GridPosition position, RoomData room, string description, string source)
        {
            if (position.X < 0 || position.Y < 0 ||
                position.X >= room.Width || position.Y >= room.Height)
            {
                throw new InvalidDataException(
                    $"Room file {source} has an invalid {description} grid position ({position.X}, {position.Y}).");
            }
        }

        public static void SetRoomData(string path, RoomData data)
        {
            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new GridPositionJsonConverter() }
            });
            File.WriteAllText(fullPath, json);
        }
    }
}