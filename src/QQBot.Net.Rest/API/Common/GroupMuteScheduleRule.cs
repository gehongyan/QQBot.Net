using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class GroupMuteScheduleRule
{
    [JsonPropertyName("task_id")]
    public string? TaskId { get; init; }

    [JsonPropertyName("start_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset StartAt { get; init; }

    [JsonPropertyName("end_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset EndAt { get; init; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }
}
