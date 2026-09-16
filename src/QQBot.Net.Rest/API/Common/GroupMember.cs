using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupMember
{
    [JsonPropertyName("member_openid")]
    [GuidJsonConverter]
    public required Guid MemberOpenId { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("member_role")]
    [JsonConverter(typeof(GroupMemberRoleJsonConverter))]
    public GroupMemberRole MemberRole { get; init; }

    [JsonPropertyName("bot")]
    public bool Bot { get; init; }

    [JsonPropertyName("joined_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset JoinedAt { get; init; }

    [JsonPropertyName("union_openid")]
    public string? UnionOpenId { get; init; }
}
