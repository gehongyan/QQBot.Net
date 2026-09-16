using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class SubMenuItemTypeJsonConverter : JsonConverter<SubMenuItemType>
{
    /// <inheritdoc />
    public override SubMenuItemType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "link" => SubMenuItemType.Link,
            _ => SubMenuItemType.SendMessage
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, SubMenuItemType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            SubMenuItemType.Link => "link",
            _ => "send_message"
        });
}
