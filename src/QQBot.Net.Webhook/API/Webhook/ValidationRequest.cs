using System.Text.Json.Serialization;

namespace QQBot.API.Webhook;

internal sealed class ValidationRequest
{
    [JsonPropertyName("plain_token")]
    public required string PlainToken { get; init; }

    [JsonPropertyName("event_ts")]
    public required string EventTimestamp { get; init; }
}
