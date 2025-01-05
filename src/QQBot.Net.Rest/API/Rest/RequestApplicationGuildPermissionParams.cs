using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class RequestApplicationGuildPermissionParams
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("api_identify")]
    public required ApiPermissionDemandIdentify ApiIdentify { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("desc")]
    public string? Description { get; init; }
}
