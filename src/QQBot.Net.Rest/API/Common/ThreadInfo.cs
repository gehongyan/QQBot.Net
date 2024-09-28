using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ThreadInfo
{
    [JsonPropertyName("thread_id")]
    public required ulong ThreadId { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("date_time")]
    public required DateTimeOffset DateTime { get; set; }
}
