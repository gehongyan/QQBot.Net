using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetBotShardedGatewayResponse : GetBotGatewayResponse
{
    [JsonPropertyName("shards")]
    public required int Shards { get; init; }

    [JsonPropertyName("session_start_limit")]
    public required SessionStartLimit SessionStartLimit { get; init; }
}
