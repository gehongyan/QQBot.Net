using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ChannelMessage
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("timestamp")]
    public required DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("edited_timestamp")]
    public DateTimeOffset? EditedTimestamp { get; set; }

    [JsonPropertyName("mention_everyone")]
    public required bool MentionEveryone { get; set; }

    [JsonPropertyName("author")]
    public required User Author { get; set; }

    [JsonPropertyName("attachments")]
    public required MessageAttachment[] Attachments { get; set; }

    [JsonPropertyName("embeds")]
    public required MessageEmbed[] Embeds { get; set; }

    [JsonPropertyName("mentions")]
    public required User[]? Mentions { get; set; }

    [JsonPropertyName("member")]
    public required Member Member { get; set; }

    [JsonPropertyName("ark")]
    public required MessageArk Ark { get; set; }

    [JsonPropertyName("seq")]
    public int? Sequence { get; set; }

    [JsonPropertyName("seq_in_channel")]
    public required string SequenceInChannel { get; set; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; set; }
}
