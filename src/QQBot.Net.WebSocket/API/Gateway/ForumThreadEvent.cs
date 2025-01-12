using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ForumThreadEvent
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("author_id")]
    public required ulong AuthorId { get; init; }

    [JsonPropertyName("thread_info")]
    public required ThreadInfo ThreadInfo { get; set; }
}
