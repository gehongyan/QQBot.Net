using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class ForumPublishAuditResultEvent
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("author_id")]
    public required ulong AuthorId { get; init; }

    [JsonPropertyName("thread_id")]
    public required string? ThreadId { get; init; }

    [JsonPropertyName("post_id")]
    public required string? PostId { get; init; }

    [JsonPropertyName("reply_id")]
    public required string? ReplyId { get; init; }

    [JsonPropertyName("type")]
    public AuditType AuditType { get; init; }

    [JsonPropertyName("result")]
    [NumberBooleanConverter]
    public bool Failed { get; init; }

    [JsonPropertyName("err_msg")]
    public string? ErrorMessage { get; init; }
}
