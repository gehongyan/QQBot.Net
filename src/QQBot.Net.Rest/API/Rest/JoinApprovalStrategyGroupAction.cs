using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class JoinApprovalStrategyGroupAction
{
    [JsonPropertyName("op")]
    public required string Op { get; init; }

    [JsonPropertyName("group_openids")]
    public string[]? GroupOpenids { get; init; }

    [JsonPropertyName("group_ids")]
    public ulong[]? GroupIds { get; init; }
}
