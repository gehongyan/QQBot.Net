using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetBotGatewayResponse
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}
