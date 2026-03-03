using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetAccessTokenResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }

    [JsonPropertyName("expires_in")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public required int ExpiresIn { get; init; }
}
