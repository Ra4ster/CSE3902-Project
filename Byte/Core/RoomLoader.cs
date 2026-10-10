using Byte.Rooms;
using System.Text.Json;

namespace Byte
{
    internal class RoomLoader
    {
        public static RoomData GetRoomData(string path)
        {
            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            using (FileStream jsonStream = new FileStream(fullPath, FileMode.Open))
            {
                JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true, AllowOutOfOrderMetadataProperties = true, };

                RoomData? data = JsonSerializer.Deserialize<RoomData>(jsonStream, options);
                return data ?? throw new InvalidDataException($"Room file {fullPath} contained no room data.");
            }
        }

        public static void SetRoomData(string path, RoomData data)
        {
            string fullPath = Path.Combine(AppContext.BaseDirectory, path);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
            });
            File.WriteAllText(fullPath, json);
        }
    }
}