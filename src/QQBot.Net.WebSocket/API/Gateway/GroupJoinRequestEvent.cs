using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class GroupJoinRequestEvent
{
    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public required Guid GroupOpenid { get; init; }

    [JsonPropertyName("join_request_id")]
    public required string JoinRequestId { get; init; }

    [JsonPropertyName("risk_tips")]
    public string? RiskTips { get; init; }

    [JsonPropertyName("union_openid")]
    public string? UnionOpenId { get; init; }

    [JsonPropertyName("member_openid")]
    [GuidJsonConverter]
    public required Guid MemberOpenId { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("apply_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset ApplyAt { get; init; }

    [JsonPropertyName("apply_source")]
    [JsonConverter(typeof(GroupJoinSourceJsonConverter))]
    public GroupJoinSource ApplySource { get; init; }

    [JsonPropertyName("invited_by")]
    [GuidJsonConverter]
    public Guid? InvitedBy { get; init; }

    [JsonPropertyName("bot")]
    public bool Bot { get; init; }

    [JsonPropertyName("verify_info")]
    public GroupJoinVerifyInfo? VerifyInfo { get; init; }

    [JsonPropertyName("auto_approved")]
    public GroupJoinAutoApproved? AutoApproved { get; init; }
}

internal class GroupJoinVerifyInfo
{
    [JsonPropertyName("method")]
    [JsonConverter(typeof(GroupJoinVerifyMethodJsonConverter))]
    public GroupJoinVerifyMethod Method { get; init; }

    [JsonPropertyName("verify_message")]
    public string? VerifyMessage { get; init; }

    [JsonPropertyName("review_qa_list")]
    public GroupJoinReviewQa[]? ReviewQaList { get; init; }
}

internal class GroupJoinReviewQa
{
    [JsonPropertyName("question")]
    public string? Question { get; init; }

    [JsonPropertyName("answer")]
    public string? Answer { get; init; }
}

internal class GroupJoinAutoApproved
{
    [JsonPropertyName("strategy_id")]
    public string? StrategyId { get; init; }
}
