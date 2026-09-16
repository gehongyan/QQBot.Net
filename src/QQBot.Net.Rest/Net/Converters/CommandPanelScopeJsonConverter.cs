using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class CommandPanelScopeJsonConverter : JsonConverter<CommandPanelScope>
{
    /// <inheritdoc />
    public override CommandPanelScope Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "group" => CommandPanelScope.Group,
            "channel" => CommandPanelScope.Channel,
            "dm" => CommandPanelScope.DM,
            _ => CommandPanelScope.C2C
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, CommandPanelScope value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            CommandPanelScope.Group => "group",
            CommandPanelScope.Channel => "channel",
            CommandPanelScope.DM => "dm",
            _ => "c2c"
        });
}
