using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class CommandPanelTargetTypeJsonConverter : JsonConverter<CommandPanelTargetType>
{
    /// <inheritdoc />
    public override CommandPanelTargetType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "specific" => CommandPanelTargetType.Specific,
            _ => CommandPanelTargetType.All
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CommandPanelTargetType value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            CommandPanelTargetType.Specific => "specific",
            _ => "all"
        });
}
