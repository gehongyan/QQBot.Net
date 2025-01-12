using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ForumPostEvent
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("author_id")]
    public required ulong AuthorId { get; init; }

    [JsonPropertyName("post_info")]
    public required PostInfo PostInfo { get; set; }
}
