using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ImageElement
{
    [JsonPropertyName("plat_image")]
    public required PlatformImage PlatformImage { get; set; }

    [JsonPropertyName("width_percent")]
    public required double Ratio { get; set; }
}
