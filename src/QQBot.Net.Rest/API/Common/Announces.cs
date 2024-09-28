using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Announces
{
    [JsonPropertyName("guild_id")]
    public ulong? GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; set; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("announces_type")]
    public required AnnouncementType AnnouncementType { get; set; }

    [JsonPropertyName("recommend_channels")]
    public required RecommendChannel[] RecommendChannels { get; set; }
}
