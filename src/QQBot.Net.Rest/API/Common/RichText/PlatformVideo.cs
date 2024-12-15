using System.Text.Json.Serialization;

namespace QQBot.API;

internal class PlatformVideo
{
    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("width")]
    public required int Width { get; set; }

    [JsonPropertyName("height")]
    public required int Height { get; set; }

    [JsonPropertyName("video_id")]
    public required string VideoId { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("cover")]
    public required PlatformVideoCover Cover { get; set; }
}
