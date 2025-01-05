using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ChannelEvent : Channel
{
    [JsonPropertyName("op_user_id")]
    public required ulong OperatorUserId { get; init; }
}
