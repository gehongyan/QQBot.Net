using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class MessageReactionEvent
{
    [JsonPropertyName("user_id")]
    public required ulong UserId { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("guild_id")]
    public required ulong GuildId { get; init; }

    [JsonPropertyName("target")]
    public required ReactionTarget Target { get; init; }

    [JsonPropertyName("emoji")]
    public required ReactionEmoji Emoji { get; init; }
}

internal class ReactionTarget
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public int Type { get; init; }
}

internal class ReactionEmoji
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("type")]
    public EmojiType Type { get; init; }
}
