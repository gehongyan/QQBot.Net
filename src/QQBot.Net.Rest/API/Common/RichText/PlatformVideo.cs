using System.Text.Json.Serialization;

namespace QQBot.API;

internal class PlatformVideo
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("width")]
    public required int Width { get; init; }

    [JsonPropertyName("height")]
    public required int Height { get; init; }

    [JsonPropertyName("video_id")]
    public required string VideoId { get; init; }

    [JsonPropertyName("duration")]
    public int? Duration { get; init; }

    [JsonPropertyName("cover")]
    public required PlatformVideoCover Cover { get; init; }
}
