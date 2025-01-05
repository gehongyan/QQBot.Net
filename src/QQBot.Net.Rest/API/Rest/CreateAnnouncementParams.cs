using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateAnnouncementParams
{
    [JsonPropertyName("message_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? ChannelId { get; init; }

    [JsonPropertyName("announces_type")]
    public AnnouncementType? AnnouncementType { get; init; }

    [JsonPropertyName("recommend_channels")]
    public RecommendChannel[]? RecommendChannels { get; init; }
}
