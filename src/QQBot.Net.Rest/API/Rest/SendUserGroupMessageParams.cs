using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendUserGroupMessageParams
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("msg_type")]
    public required MessageType MessageType { get; set; }

    [JsonPropertyName("markdown")]
    public MessageMarkdown? Markdown { get; set; }

    [JsonPropertyName("keyboard")]
    public Keyboard? Keyboard { get; set; }

    [JsonPropertyName("ark")]
    public MessageArk? Ark { get; set; }

    [JsonPropertyName("media")]
    public MediaFileInfo? MediaFileInfo { get; set; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; set; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    [JsonPropertyName("msg_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("msg_seq")]
    public int? MessageSequence { get; set; }
}
