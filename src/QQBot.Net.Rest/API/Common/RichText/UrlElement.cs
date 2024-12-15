using System.Text.Json.Serialization;

namespace QQBot.API;

internal class UrlElement
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}