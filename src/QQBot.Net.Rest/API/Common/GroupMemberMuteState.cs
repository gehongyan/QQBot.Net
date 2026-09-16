using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupMemberMuteState
{
    [JsonPropertyName("member_openid")]
    [GuidJsonConverter]
    public required Guid MemberOpenId { get; init; }

    [JsonPropertyName("mute_expire_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset MuteExpireAt { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("union_openid")]
    public string? UnionOpenId { get; init; }
}
