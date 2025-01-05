using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetBotGatewayResponse
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("shards")]
    public required int Shards { get; init; }

    [JsonPropertyName("session_start_limit")]
    public required SessionStartLimit SessionStartLimit { get; init; }
}
