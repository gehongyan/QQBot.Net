using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class SendUserGroupMessageResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset Timestamp { get; set; }
}
