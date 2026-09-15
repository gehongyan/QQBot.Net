using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class PrepareMediaUploadConfiguration
{
    [JsonPropertyName("concurrency")]
    public int? Concurrency { get; init; }
    [JsonPropertyName("retry_timeout")]
    public int? RetryTimeout { get; init; }
    [JsonPropertyName("retry_delay")]
    public int? RetryDelay { get; init; }
}