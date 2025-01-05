using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ImageElement
{
    [JsonPropertyName("plat_image")]
    public PlatformImage? PlatformImage { get; init; }

    [JsonPropertyName("third_url")]
    public string? Url { get; init; }

    [JsonPropertyName("width_percent")]
    public required double Ratio { get; init; }
}
