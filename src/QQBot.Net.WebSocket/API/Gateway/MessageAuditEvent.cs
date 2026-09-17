using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class MessageAuditEvent
{
    [JsonPropertyName("audit_id")]
    public required string AuditId { get; init; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("audit_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.ISO8601)]
    public DateTimeOffset AuditTime { get; init; }

    [JsonPropertyName("create_time")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.ISO8601)]
    public DateTimeOffset CreateTime { get; init; }

    [JsonPropertyName("seq_in_channel")]
    public string? SequenceInChannel { get; init; }
}
