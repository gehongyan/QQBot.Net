using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class OperateJoinApprovalWhitelistParams
{
    [JsonPropertyName("op")]
    public required string Op { get; init; }

    [JsonPropertyName("whitelist_users")]
    public required string[] WhitelistUsers { get; init; }
}
