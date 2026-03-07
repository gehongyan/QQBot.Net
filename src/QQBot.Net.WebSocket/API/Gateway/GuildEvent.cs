using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class GuildEvent : Guild
{
    [JsonPropertyName("op_user_id")]
    public required ulong OperatorUserId { get; init; }
}
