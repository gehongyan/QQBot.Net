using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageSetting
{
    [JsonPropertyName("disable_create_dm")]
    public required bool DisableCreateDirectMessage { get; init; }

    [JsonPropertyName("disable_push_msg")]
    public required bool DisablePushMessage { get; init; }

    [JsonPropertyName("channel_ids")]
    public required ulong[] ChannelIds { get; init; }

    [JsonPropertyName("channel_push_max_num")]
    public required uint ChannelPushMaxNumber { get; init; }
}
