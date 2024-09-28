using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageAudited
{
    [JsonPropertyName("audit_id")]
    public required ulong AuditId { get; set; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("audit_time")]
    public required DateTimeOffset AuditTime { get; set; }

    [JsonPropertyName("create_time")]
    public required DateTimeOffset CreateTime { get; set; }

    [JsonPropertyName("seq_in_channel")]
    public required string SequenceInChannel { get; set; }
}
