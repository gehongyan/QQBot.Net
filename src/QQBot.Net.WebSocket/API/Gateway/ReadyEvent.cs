using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ReadyEvent
{
    [JsonPropertyName("version")]
    public required int Version { get; init; }

    [JsonPropertyName("session_id")]
    public required Guid SessionId { get; init; }

    [JsonPropertyName("user")]
    public required User User { get; init; }

    [JsonPropertyName("shard")]
    public required int[] ShardingParams { get; init; }
}
