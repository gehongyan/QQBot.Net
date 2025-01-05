using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class GatewaySocketFrame
{
    [JsonPropertyName("op")]
    public GatewayOpCode OpCode { get; init; }

    [JsonPropertyName("s")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Sequence { get; init; }

    [JsonPropertyName("t")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; init; }

    [JsonPropertyName("d")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Payload { get; init; }
}
