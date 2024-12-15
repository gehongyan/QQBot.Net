using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Element
{
    [JsonPropertyName("text")]
    public TextElement? Text { get; set; }

    [JsonPropertyName("image")]
    public ImageElement? Image { get; set; }

    [JsonPropertyName("video")]
    public VideoElement? Video { get; set; }

    [JsonPropertyName("url")]
    public UrlElement? Url { get; set; }

    [JsonPropertyName("type")]
    public ElementType? ElementType { get; set; }
}
