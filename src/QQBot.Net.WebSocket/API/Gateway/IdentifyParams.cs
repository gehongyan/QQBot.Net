using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class IdentifyParams
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }

    [JsonPropertyName("intents")]
    public required int Intents { get; set; }

    [JsonPropertyName("shard")]
    public required int[] ShardingParams { get; set; }

    [JsonPropertyName("properties")]
    public required IDictionary<string, string> Properties { get; set; }
}
