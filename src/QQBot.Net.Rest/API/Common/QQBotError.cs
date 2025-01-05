using System.Text.Json.Serialization;

namespace QQBot.API;

internal class QQBotError
{
    [JsonPropertyName("code")]
    public required QQBotErrorCode Code { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }

    [JsonPropertyName("err_code")]
    public int? ErrorCode { get; init; }

    [JsonPropertyName("trace_id")]
    public string? TraceId { get; init; }
}
