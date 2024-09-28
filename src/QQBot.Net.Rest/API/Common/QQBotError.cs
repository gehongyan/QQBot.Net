using System.Text.Json.Serialization;

namespace QQBot.API;

internal class QQBotError
{
    [JsonPropertyName("code")]
    public required QQBotErrorCode Code { get; set; }

    [JsonPropertyName("message")]
    public required string Message { get; set; }

    [JsonPropertyName("err_code")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; }
}
