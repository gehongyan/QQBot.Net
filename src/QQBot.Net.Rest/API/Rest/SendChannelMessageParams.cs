using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendChannelMessageParams
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("embed")]
    public MessageEmbed? Embed { get; set; }

    [JsonPropertyName("ark")]
    public MessageArk? Ark { get; set; }

    [JsonPropertyName("message_reference")]
    public MessageReference? MessageReference { get; set; }

    [JsonPropertyName("event_image")]
    public string? Image { get; set; }

    [JsonPropertyName("msg_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    [JsonPropertyName("markdown")]
    public MessageMarkdown? Markdown { get; set; }
}
