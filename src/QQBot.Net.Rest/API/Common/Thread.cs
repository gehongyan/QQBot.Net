using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Thread
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("author_id")]
    public required ulong AuthorId { get; set; }

    [JsonPropertyName("thread_info")]
    public required ThreadInfo ThreadInfo { get; set; }
}
