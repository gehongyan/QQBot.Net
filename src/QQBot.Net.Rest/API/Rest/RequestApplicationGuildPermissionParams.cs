using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class RequestApplicationGuildPermissionParams
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("api_identify")]
    public required ApiPermissionDemandIdentify ApiIdentify { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("desc")]
    public string? Description { get; set; }
}
