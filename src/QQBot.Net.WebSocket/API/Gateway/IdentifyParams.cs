using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class IdentifyParams
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("intents")]
    public required int Intents { get; init; }

    [JsonPropertyName("shard")]
    public required int[] ShardingParams { get; init; }

    [JsonPropertyName("properties")]
    public required IDictionary<string, string> Properties { get; init; }
}
