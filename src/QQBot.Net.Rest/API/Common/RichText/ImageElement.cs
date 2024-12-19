using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ImageElement
{
    [JsonPropertyName("plat_image")]
    public PlatformImage? PlatformImage { get; set; }

    [JsonPropertyName("third_url")]
    public string? Url { get; set; }

    [JsonPropertyName("width_percent")]
    public required double Ratio { get; set; }
}
