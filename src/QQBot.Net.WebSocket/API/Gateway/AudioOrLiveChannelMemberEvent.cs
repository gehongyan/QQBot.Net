using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class AudioOrLiveChannelMemberEvent
{
    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("channel_type")]
    public AudioOrLiveChannelType ChannelType { get; init; }

    [JsonPropertyName("user_id")]
    public required ulong UserId { get; init; }
}