using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageAttachment
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
