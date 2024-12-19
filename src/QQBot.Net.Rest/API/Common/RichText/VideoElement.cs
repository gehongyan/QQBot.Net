using System.Text.Json.Serialization;

namespace QQBot.API;

internal class VideoElement
{
    [JsonPropertyName("plat_video")]
    public PlatformVideo? PlatformVideo { get; set; }

    [JsonPropertyName("third_url")]
    public string? Url { get; set; }
}
