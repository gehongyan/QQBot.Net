using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ApiPermissionDemand
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("api_identify")]
    public required ApiPermissionDemandIdentify ApiIdentify { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("desc")]
    public required string Description { get; init; }
}

internal class ApiPermissionDemandIdentify
{
    [JsonPropertyName("path")]
    public required string Path { get; init; }

    [JsonPropertyName("method")]
    public required string Method { get; init; }
}
