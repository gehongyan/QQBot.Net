using System.Text.Json.Serialization;

namespace QQBot.API;

internal class RecommendChannel
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("channel_introduce")]
    public required string Introduce { get; init; }
}
