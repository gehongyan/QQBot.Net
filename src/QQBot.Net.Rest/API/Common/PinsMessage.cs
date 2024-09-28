using System.Text.Json.Serialization;

namespace QQBot.API;

internal class PinsMessage
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("message_ids")]
    public required ulong[] MessageIds { get; set; }
}
