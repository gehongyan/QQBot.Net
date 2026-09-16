using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class GroupGlobalMuteModeJsonConverter : JsonConverter<GroupGlobalMuteMode>
{
    /// <inheritdoc />
    public override GroupGlobalMuteMode Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "always" => GroupGlobalMuteMode.Always,
            "schedule" => GroupGlobalMuteMode.Schedule,
            _ => GroupGlobalMuteMode.None
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GroupGlobalMuteMode value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            GroupGlobalMuteMode.Always => "always",
            GroupGlobalMuteMode.Schedule => "schedule",
            _ => "none"
        });
}
