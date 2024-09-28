using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateAnnouncementParams
{
    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }

    [JsonPropertyName("announces_type")]
    public AnnouncementType? AnnouncementType { get; set; }

    [JsonPropertyName("recommend_channels")]
    public RecommendChannel[]? RecommendChannels { get; set; }
}
