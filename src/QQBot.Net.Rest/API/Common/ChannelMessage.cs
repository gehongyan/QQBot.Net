using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class ChannelMessage
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.ISO8601)]
    public required DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("edited_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.ISO8601)]
    public DateTimeOffset? EditedTimestamp { get; set; }

    [JsonPropertyName("mention_everyone")]
    public bool? MentionEveryone { get; set; }

    [JsonPropertyName("author")]
    public required User Author { get; set; }

    [JsonPropertyName("attachments")]
    public MessageAttachment[]? Attachments { get; set; }

    [JsonPropertyName("embeds")]
    public MessageEmbed[]? Embeds { get; set; }

    [JsonPropertyName("mentions")]
    public User[]? Mentions { get; set; }

    [JsonPropertyName("member")]
    public Member? Member { get; set; }

    [JsonPropertyName("ark")]
    public MessageArk? Ark { get; set; }

    [JsonPropertyName("seq")]
    public int? Sequence { get; set; }

    [JsonPropertyName("seq_in_channel")]
    public required string SequenceInChannel { get; set; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; set; }
}
