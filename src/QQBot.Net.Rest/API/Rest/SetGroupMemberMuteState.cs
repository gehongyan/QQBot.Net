using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SetGroupMemberMuteState
{
    [JsonPropertyName("op")]
    public required string Op { get; init; }

    [JsonPropertyName("member_openid")]
    public required string MemberOpenId { get; init; }

    [JsonPropertyName("mute_expire_at")]
    public string? MuteExpireAt { get; init; }
}
