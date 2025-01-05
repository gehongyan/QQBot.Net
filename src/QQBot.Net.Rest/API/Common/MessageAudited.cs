using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageAudited
{
    [JsonPropertyName("audit_id")]
    public required ulong AuditId { get; init; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("audit_time")]
    public required DateTimeOffset AuditTime { get; init; }

    [JsonPropertyName("create_time")]
    public required DateTimeOffset CreateTime { get; init; }

    [JsonPropertyName("seq_in_channel")]
    public required string SequenceInChannel { get; init; }
}
