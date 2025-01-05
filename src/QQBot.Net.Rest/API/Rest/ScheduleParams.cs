using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class ScheduleParams
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("start_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.MillisecondsString)]
    public required DateTimeOffset StartTimestamp { get; init; }

    [JsonPropertyName("end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.MillisecondsString)]
    public required DateTimeOffset EndTimestamp { get; init; }

    [JsonPropertyName("jump_channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? JumpChannelId { get; init; }

    [JsonPropertyName("remind_type")]
    [JsonConverter(typeof(EnumNumberStringJsonConverter<RemindType>))]
    public required RemindType RemindType { get; init; }
}
