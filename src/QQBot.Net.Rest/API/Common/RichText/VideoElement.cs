using System.Text.Json.Serialization;

namespace QQBot.API;

internal class VideoElement
{
    [JsonPropertyName("plat_video")]
    public PlatformVideo? PlatformVideo { get; init; }

    [JsonPropertyName("third_url")]
    public string? Url { get; init; }
}
