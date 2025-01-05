using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class MuteMemberParams
{
    [JsonPropertyName("mute_end_timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Seconds)]
    public DateTimeOffset? MuteEndTimestamp { get; init; }

    [JsonPropertyName("mute_seconds")]
    [TimeSpanNumberJsonConverter(Unit = TimeSpanNumberJsonConverter.TimeSpanUnit.Seconds)]
    public TimeSpan? MuteSeconds { get; init; }
}
