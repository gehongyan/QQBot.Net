using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetAccessTokenParams
{
    [JsonPropertyName("appId")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public required int AppId { get; set; }

    [JsonPropertyName("clientSecret")]
    public required string ClientSecret { get; set; }
}
