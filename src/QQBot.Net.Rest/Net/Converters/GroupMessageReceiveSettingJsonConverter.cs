using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.Net.Converters;

internal class GroupMessageReceiveSettingJsonConverter : JsonConverter<GroupMessageReceiveSetting>
{
    /// <inheritdoc />
    public override GroupMessageReceiveSetting Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "only_mention" => GroupMessageReceiveSetting.OnlyMention,
            "mention_and_context" => GroupMessageReceiveSetting.MentionAndContext,
            _ => GroupMessageReceiveSetting.All
        };

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, GroupMessageReceiveSetting value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            GroupMessageReceiveSetting.OnlyMention => "only_mention",
            GroupMessageReceiveSetting.MentionAndContext => "mention_and_context",
            _ => "all"
        });
}
