using System.Text.Json.Serialization;

namespace QQBot.API;

internal class ApiPermissionDemand
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("api_identify")]
    public required ApiPermissionDemandIdentify ApiIdentify { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}

internal class ApiPermissionDemandIdentify
{
    [JsonPropertyName("path")]
    public required string Path { get; set; }

    [JsonPropertyName("method")]
    public required string Method { get; set; }
}
