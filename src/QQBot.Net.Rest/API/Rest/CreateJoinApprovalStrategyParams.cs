using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateJoinApprovalStrategyParams
{
    [JsonPropertyName("group_openids")]
    public string[]? GroupOpenids { get; init; }

    [JsonPropertyName("group_ids")]
    public ulong[]? GroupIds { get; init; }

    [JsonPropertyName("is_enable")]
    public string? IsEnable { get; init; }

    [JsonPropertyName("expire_at")]
    public string? ExpireAt { get; init; }

    [JsonPropertyName("remark")]
    public string? Remark { get; init; }
}
