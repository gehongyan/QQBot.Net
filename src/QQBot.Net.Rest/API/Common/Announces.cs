using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Announces
{
    [JsonPropertyName("guild_id")]
    public ulong? GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public ulong? ChannelId { get; init; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("announces_type")]
    public required AnnouncementType AnnouncementType { get; init; }

    [JsonPropertyName("recommend_channels")]
    public required RecommendChannel[] RecommendChannels { get; init; }
}
