using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ResumeParams
{
    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("session_id")]
    public required Guid SessionId { get; init; }

    [JsonPropertyName("seq")]
    public required int Sequence { get; init; }
}
