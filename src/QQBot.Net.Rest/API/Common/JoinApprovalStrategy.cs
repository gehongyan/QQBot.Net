using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class JoinApprovalStrategy
{
    [JsonPropertyName("strategy_id")]
    public required string StrategyId { get; init; }

    [JsonPropertyName("group_openids")]
    public string[]? GroupOpenids { get; init; }

    [JsonPropertyName("group_ids")]
    public ulong[]? GroupIds { get; init; }

    [JsonPropertyName("whitelist_user_count")]
    public int WhitelistUserCount { get; init; }

    [JsonPropertyName("is_enable")]
    public string? IsEnable { get; init; }

    [JsonPropertyName("expire_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset ExpireAt { get; init; }

    [JsonPropertyName("created_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.RFC3339)]
    public DateTimeOffset UpdatedAt { get; init; }

    [JsonPropertyName("remark")]
    public string? Remark { get; init; }
}
