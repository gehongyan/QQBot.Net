using System.Text.Json.Serialization;

namespace QQBot.API;

internal class UrlElement
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("desc")]
    public required string Description { get; init; }
}
