using System.Text.Json.Serialization;

namespace QQBot.API;

internal class PlatformImage
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("width")]
    public required int Width { get; set; }

    [JsonPropertyName("height")]
    public required int Height { get; set; }

    [JsonPropertyName("image_id")]
    public required string ImageId { get; set; }
}
