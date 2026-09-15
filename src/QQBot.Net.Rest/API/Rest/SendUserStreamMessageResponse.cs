using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class SendUserStreamMessageResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("timestamp")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public required DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("ext_info")]
    public SendUserGroupMessageResponseExtInfo? ExtInfo { get; init; }

    [JsonPropertyName("remain_msg_len")]
    public int? RemainingMessageLength { get; init; }
}
