using System.Text.Json.Serialization;

namespace QQBot.API;

internal class PostInfo
{
    [JsonPropertyName("thread_id")]
    public required string ThreadId { get; init; }

    [JsonPropertyName("post_id")]
    public required string PostId { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("date_time")]
    public required DateTimeOffset DateTime { get; init; }
}
