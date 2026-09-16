using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ModifyJoinApprovalStrategyParams
{
    [JsonPropertyName("is_enable")]
    public string? IsEnable { get; init; }

    [JsonPropertyName("expire_at")]
    public string? ExpireAt { get; init; }

    [JsonPropertyName("group_action")]
    public JoinApprovalStrategyGroupAction? GroupAction { get; init; }

    [JsonPropertyName("remark")]
    public string? Remark { get; init; }
}
