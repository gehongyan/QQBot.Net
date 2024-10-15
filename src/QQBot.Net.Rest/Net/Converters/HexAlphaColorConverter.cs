using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class HexAlphaColorConverter : JsonConverter<AlphaColor>
{
    public override AlphaColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        new(reader.GetUInt32());

    public override void Write(Utf8JsonWriter writer, AlphaColor value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.RawValue);
}
