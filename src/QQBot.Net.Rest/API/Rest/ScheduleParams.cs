using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class ScheduleParams
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("start_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds)]
    public DateTimeOffset? StartTimestamp { get; set; }

    [JsonPropertyName("end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds)]
    public DateTimeOffset? EndTimestamp { get; set; }

    [JsonPropertyName("creator")]
    public Member? Creator { get; set; }

    [JsonPropertyName("jump_channel_id")]
    public ulong? JumpChannelId { get; set; }

    [JsonPropertyName("remind_type")]
    public RemindType? RemindType { get; set; }
}