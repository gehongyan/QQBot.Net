using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetJoinApprovalStrategyListResponse
{
    [JsonPropertyName("strategies")]
    public required JoinApprovalStrategy[] Strategies { get; init; }

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; init; }
}
