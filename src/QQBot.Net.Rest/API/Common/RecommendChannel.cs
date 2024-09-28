using System.Text.Json.Serialization;

namespace QQBot.API;

internal class RecommendChannel
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("channel_introduce")]
    public required string Introduce { get; set; }
}
