using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SessionStartLimit
{
    [JsonPropertyName("total")]
    public required int Total { get; init; }

    [JsonPropertyName("remaining")]
    public required int Remaining { get; init; }

    [JsonPropertyName("reset_after")]
    public required int ResetAfter { get; init; }

    [JsonPropertyName("max_concurrency")]
    public required int MaxConcurrency { get; init; }
}
