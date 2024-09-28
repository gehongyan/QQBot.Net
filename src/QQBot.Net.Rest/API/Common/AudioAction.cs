using System.Text.Json.Serialization;

namespace QQBot.API;

internal class AudioAction
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; set; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; set; }

    [JsonPropertyName("audio_url")]
    public string? AudioUrl { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
