using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class GroupJoinSourceJsonConverter : JsonConverter<GroupJoinSource>
{
    /// <inheritdoc />
    public override GroupJoinSource Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "invited" => GroupJoinSource.Invited,
            _ => GroupJoinSource.SelfApply
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GroupJoinSource value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            GroupJoinSource.Invited => "invited",
            _ => "self_apply"
        });
}
