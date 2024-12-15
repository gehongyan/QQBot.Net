using System.Text.Json.Serialization;

namespace QQBot.API;

internal class VideoElement
{
    [JsonPropertyName("plat_video")]
    public required PlatformVideo PlatformVideo { get; set; }
}
