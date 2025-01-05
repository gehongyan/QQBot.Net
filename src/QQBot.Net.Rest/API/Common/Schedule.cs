using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class Schedule
{
    [JsonPropertyName("id")]
    public required ulong Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("start_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds)]
    public required DateTimeOffset StartTimestamp { get; init; }

    [JsonPropertyName("end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Milliseconds)]
    public required DateTimeOffset EndTimestamp { get; init; }

    [JsonPropertyName("creator")]
    public required Member Creator { get; init; }

    [JsonPropertyName("jump_channel_id")]
    public required ulong? JumpChannelId { get; init; }

    [JsonPropertyName("remind_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required RemindType RemindType { get; init; }
}
