using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class ReadyEvent
{
    [JsonPropertyName("version")]
    public required int Version { get; set; }

    [JsonPropertyName("session_id")]
    public required Guid SessionId { get; set; }

    [JsonPropertyName("user")]
    public required User User { get; set; }

    [JsonPropertyName("shard")]
    public required int[] ShardingParams { get; set; }
}
