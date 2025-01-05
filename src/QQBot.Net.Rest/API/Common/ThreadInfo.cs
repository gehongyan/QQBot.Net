using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ThreadInfo
{
    [JsonPropertyName("thread_id")]
    public required string ThreadId { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("date_time")]
    public required DateTimeOffset DateTime { get; init; }
}
