using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class MessageCreatedEvent
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("author")]
    public required Author Author { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("group_id")]
    [GuidJsonConverter]
    public Guid? GroupId { get; init; }

    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public Guid? GroupOpenId { get; init; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("attachments")]
    public Attachment[]? Attachments { get; init; }
}
