using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class ChannelApplicationJsonConverter : JsonConverter<ChannelApplication?>
{
    /// <inheritdoc />
    public override bool HandleNull => true;

    /// <inheritdoc />
    public override ChannelApplication? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.GetString() is { } value
            && int.TryParse(value, out int result)
            && result != 0)
            return (ChannelApplication)result;
        return null;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ChannelApplication? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int?)value ?? 0).ToString());
    }
}
