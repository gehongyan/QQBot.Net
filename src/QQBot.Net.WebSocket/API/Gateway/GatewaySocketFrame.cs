using System.Text.Json;
using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class GatewaySocketFrame
{
    [JsonPropertyName("op")]
    public GatewayOpCode OpCode { get; set; }

    [JsonPropertyName("s")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Sequence { get; set; }

    [JsonPropertyName("t")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    [JsonPropertyName("d")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Payload { get; set; }
}
