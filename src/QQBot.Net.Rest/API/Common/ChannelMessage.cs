using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class ChannelMessage
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.ISO8601)]
    public required DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("edited_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.ISO8601)]
    public DateTimeOffset? EditedTimestamp { get; init; }

    [JsonPropertyName("mention_everyone")]
    public bool? MentionEveryone { get; init; }

    [JsonPropertyName("author")]
    public required User Author { get; init; }

    [JsonPropertyName("attachments")]
    public MessageAttachment[]? Attachments { get; init; }

    [JsonPropertyName("embeds")]
    public MessageEmbed[]? Embeds { get; init; }

    [JsonPropertyName("mentions")]
    public User[]? Mentions { get; init; }

    [JsonPropertyName("member")]
    public Member? Member { get; init; }

    [JsonPropertyName("ark")]
    public MessageArk? Ark { get; init; }

    [JsonPropertyName("seq")]
    public int? Sequence { get; init; }

    [JsonPropertyName("seq_in_channel")]
    public required string SequenceInChannel { get; init; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; init; }
}
