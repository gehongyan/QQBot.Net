using System.Text.Json.Serialization;

namespace QQBot.API.Webhook;

internal sealed class CallbackAck
{
    [JsonPropertyName("op")]
    public required int OpCode { get; init; }

    [JsonPropertyName("d")]
    public required int Data { get; init; }
}
