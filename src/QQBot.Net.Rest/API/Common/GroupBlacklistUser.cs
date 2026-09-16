using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupBlacklistUser
{
    [JsonPropertyName("union_openid")]
    public string? UnionOpenId { get; init; }

    [JsonPropertyName("member_openid")]
    [GuidJsonConverter]
    public required Guid MemberOpenId { get; init; }

    [JsonPropertyName("username")]
    public string? Username { get; init; }

    [JsonPropertyName("banned_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset BannedAt { get; init; }

    [JsonPropertyName("bot")]
    public bool Bot { get; init; }
}
