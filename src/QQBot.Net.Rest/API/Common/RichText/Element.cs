using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Element
{
    [JsonPropertyName("text")]
    public TextElement? Text { get; init; }

    [JsonPropertyName("image")]
    public ImageElement? Image { get; init; }

    [JsonPropertyName("video")]
    public VideoElement? Video { get; init; }

    [JsonPropertyName("url")]
    public UrlElement? Url { get; init; }

    [JsonPropertyName("type")]
    public ElementType? ElementType { get; init; }
}
