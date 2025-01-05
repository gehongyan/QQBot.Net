using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendUserGroupMessageParams
{
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("msg_type")]
    public required MessageType MessageType { get; init; }

    [JsonPropertyName("markdown")]
    public MessageMarkdown? Markdown { get; init; }

    [JsonPropertyName("keyboard")]
    public Keyboard? Keyboard { get; init; }

    [JsonPropertyName("ark")]
    public MessageArk? Ark { get; init; }

    [JsonPropertyName("media")]
    public MediaFileInfo? MediaFileInfo { get; init; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; init; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; init; }

    [JsonPropertyName("msg_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("msg_seq")]
    public int? MessageSequence { get; init; }
}
