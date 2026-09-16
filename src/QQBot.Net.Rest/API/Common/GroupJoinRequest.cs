using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupJoinRequest
{
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
}
