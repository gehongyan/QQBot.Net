using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class ApproveGroupJoinRequestParams
{
    [JsonPropertyName("op")]
    public required string Op { get; init; }

    [JsonPropertyName("join_request_id")]
    public string? JoinRequestId { get; init; }

    [JsonPropertyName("reject_reason")]
    public string? RejectReason { get; init; }

    [JsonPropertyName("add_to_member_blacklist")]
    public bool? AddToMemberBlacklist { get; init; }
}
