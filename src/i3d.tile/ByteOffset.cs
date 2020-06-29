using System.Text.Json.Serialization;

namespace I3dm.Tile
{
    public class ByteOffset
    {
        [JsonPropertyName("byteOffset")]

        public int offset { get; set; }

        public string componentType { get; set; }
    }
}
