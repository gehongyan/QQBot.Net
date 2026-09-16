using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Rest;

internal class JoinApprovalStrategyMutationResponse
{
    [JsonPropertyName("strategy_id")]
    public string? StrategyId { get; init; }

    [JsonPropertyName("is_enable")]
    public string? IsEnable { get; init; }

    [JsonPropertyName("expire_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset? ExpireAt { get; init; }
}
