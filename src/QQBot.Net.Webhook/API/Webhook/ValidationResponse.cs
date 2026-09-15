using System.Text.Json.Serialization;

namespace QQBot.API.Webhook;

internal sealed class ValidationResponse
{
    [JsonPropertyName("plain_token")]
    public required string PlainToken { get; init; }

    [JsonPropertyName("signature")]
    public required string Signature { get; init; }
}
