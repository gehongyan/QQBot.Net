using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GenerateUrlLinkResponse
{
    [JsonPropertyName("retcode")]
    public required int ReturnCode { get; init; }

    [JsonPropertyName("msg")]
    public required string Message { get; init; }

    [JsonPropertyName("data")]
    public GenerateUrlLinkResponseData? Data { get; init; }
}

internal class GenerateUrlLinkResponseData
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}
