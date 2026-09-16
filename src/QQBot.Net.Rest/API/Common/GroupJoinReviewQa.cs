using System.Text.Json.Serialization;

namespace QQBot.API;

internal class GroupJoinReviewQa
{
    [JsonPropertyName("question")]
    public string? Question { get; init; }

    [JsonPropertyName("answer")]
    public string? Answer { get; init; }
}
