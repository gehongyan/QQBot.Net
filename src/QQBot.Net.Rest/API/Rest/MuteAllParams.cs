using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class MuteAllParams
{
    [JsonPropertyName("mute_end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Seconds)]
    public DateTimeOffset? MuteEndTimestamp { get; set; }

    [JsonPropertyName("mute_seconds")]
    [TimeSpanNumberJsonConverter(Unit = TimeSpanNumberJsonConverter.TimeSpanUnit.Seconds)]
    public TimeSpan? MuteSeconds { get; set; }
}
