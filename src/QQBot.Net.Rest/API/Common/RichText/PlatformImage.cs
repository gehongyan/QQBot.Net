using System.Text.Json.Serialization;

namespace QQBot.API;

internal class PlatformImage
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("width")]
    public required int Width { get; init; }

    [JsonPropertyName("height")]
    public required int Height { get; init; }

    [JsonPropertyName("image_id")]
    public required string ImageId { get; init; }
}
