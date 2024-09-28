using System.Text.Json.Serialization;

namespace QQBot.API;

internal class AudioControl
{
    [JsonPropertyName("audio_url")]
    public string? AudioUrl { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("status")]
    public required AudioStatus Status { get; set; }
}
