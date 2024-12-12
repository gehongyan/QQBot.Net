using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class ScheduleParams
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("start_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.MillisecondsString)]
    public required DateTimeOffset StartTimestamp { get; set; }

    [JsonPropertyName("end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.MillisecondsString)]
    public required DateTimeOffset EndTimestamp { get; set; }

    [JsonPropertyName("jump_channel_id")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public ulong? JumpChannelId { get; set; }

    [JsonPropertyName("remind_type")]
    [JsonConverter(typeof(EnumNumberStringJsonConverter<RemindType>))]
    public required RemindType RemindType { get; set; }
}
