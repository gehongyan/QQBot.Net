using System.Text.Json.Serialization;

namespace QQBot.API;

internal class GroupMuteRecurringRule
{
    [JsonPropertyName("task_id")]
    public string? TaskId { get; init; }

    [JsonPropertyName("weekdays")]
    public int[]? Weekdays { get; init; }

    [JsonPropertyName("start_time")]
    public string? StartTime { get; init; }

    [JsonPropertyName("end_time")]
    public string? EndTime { get; init; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }
}
