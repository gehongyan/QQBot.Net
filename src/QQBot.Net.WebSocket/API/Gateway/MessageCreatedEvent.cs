using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class MessageCreatedEvent
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("author")]
    public required Author Author { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("group_id")]
    [GuidJsonConverter]
    public Guid? GroupId { get; set; }

    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public Guid? GroupOpenId { get; set; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("attachments")]
    public Attachment[]? Attachments { get; set; }
}
