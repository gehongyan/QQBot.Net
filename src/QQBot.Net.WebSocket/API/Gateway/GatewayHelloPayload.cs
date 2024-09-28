using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class GatewayHelloPayload
{
    [JsonPropertyName("heartbeat_interval")]
    public required int HeartbeatInterval { get; set; }
}
