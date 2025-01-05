using System.Text.Json.Serialization;

namespace QQBot.API;

internal class AudioAction
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("audio_url")]
    public string? AudioUrl { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }
}
