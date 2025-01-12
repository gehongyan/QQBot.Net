using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class GroupBotEvent
{
    [JsonPropertyName("group_openid")]
    public required Guid GroupOpenid { get; init; }

    [JsonPropertyName("op_member_openid")]
    public required string OpMemberOpenId { get; init; }

    [JsonPropertyName("timestamp")]
    public required int Timestamp { get; init; }
}
