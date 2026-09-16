using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class OperateGroupBlacklistParams
{
    [JsonPropertyName("op")]
    public required string Op { get; init; }

    [JsonPropertyName("member_openids")]
    public required string[] MemberOpenIds { get; init; }
}
