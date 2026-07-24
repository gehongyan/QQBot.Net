using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class GroupMemberEvent
{
    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public required Guid GroupOpenid { get; init; }

    [JsonPropertyName("member_openid")]
    public string? MemberOpenId { get; init; }

    [JsonPropertyName("user_openid")]
    public string? UserOpenId { get; init; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; init; }

    [JsonPropertyName("op_member_openid")]
    public string? OpMemberOpenId { get; init; }

    [JsonPropertyName("operator_id")]
    public string? OperatorId { get; init; }

    [JsonPropertyName("timestamp")]
    public required int Timestamp { get; init; }
}
