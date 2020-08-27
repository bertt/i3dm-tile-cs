using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace I3dm.Tile
{
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void Write(Utf8JsonWriter writer, Vector3 vector, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(vector.X);
            writer.WriteNumberValue(vector.Y);
            writer.WriteNumberValue(vector.Z);
            writer.WriteEndArray();
        }

        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                var elements = new List<float>();

                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    var val = JsonSerializer.Deserialize<float>(ref reader, options);
                    elements.Add(val);
                    reader.Read();
                }

                return new Vector3(elements[0], elements[1], elements[2]);
            }
            throw new JsonException();
        }
    }
}
