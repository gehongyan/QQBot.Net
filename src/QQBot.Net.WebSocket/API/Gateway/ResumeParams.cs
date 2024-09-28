using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ResumeParams
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }

    [JsonPropertyName("session_id")]
    public required Guid SessionId { get; set; }

    [JsonPropertyName("seq")]
    public required int Sequence { get; set; }
}
