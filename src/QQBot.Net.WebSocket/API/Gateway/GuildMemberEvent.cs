using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class GuildMemberEvent : MemberWithGuildId
{
    [JsonPropertyName("op_user_id")]
    public required ulong OperatorUserId { get; init; }
}
