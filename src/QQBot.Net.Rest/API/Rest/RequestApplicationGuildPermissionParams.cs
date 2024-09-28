using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class RequestApplicationGuildPermissionParams
{
    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("api_identify")]
    public required ApiPermissionDemandIdentify ApiIdentify { get; set; }

    [JsonPropertyName("desc")]
    public string? Description { get; set; }
}
