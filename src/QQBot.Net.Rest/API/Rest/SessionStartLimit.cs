using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SessionStartLimit
{
    [JsonPropertyName("total")]
    public required int Total { get; set; }

    [JsonPropertyName("remaining")]
    public required int Remaining { get; set; }

    [JsonPropertyName("reset_after")]
    public required int ResetAfter { get; set; }

    [JsonPropertyName("max_concurrency")]
    public required int MaxConcurrency { get; set; }
}