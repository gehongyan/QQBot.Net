using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class CommandPanelItemTypeJsonConverter : JsonConverter<CommandPanelItemType>
{
    /// <inheritdoc />
    public override CommandPanelItemType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "link" => CommandPanelItemType.Link,
            _ => CommandPanelItemType.Command
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CommandPanelItemType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            CommandPanelItemType.Link => "link",
            _ => "command"
        });
}
