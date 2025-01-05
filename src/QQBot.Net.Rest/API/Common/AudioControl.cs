using System.Text.Json.Serialization;

namespace QQBot.API;

internal class AudioControl
{
    [JsonPropertyName("audio_url")]
    public string? AudioUrl { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("status")]
    public required AudioStatus Status { get; init; }
}
