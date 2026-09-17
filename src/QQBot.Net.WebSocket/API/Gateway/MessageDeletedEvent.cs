using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class MessageDeletedEvent
{
    [JsonPropertyName("message")]
    public required DeletedMessage Message { get; init; }

    [JsonPropertyName("op_user")]
    public required MessageDeleteOperator OpUser { get; init; }
}

internal class DeletedMessage
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("channel_id")]
    public required ulong ChannelId { get; init; }

    [JsonPropertyName("guild_id")]
    public ulong? GuildId { get; init; }

    [JsonPropertyName("author")]
    public User? Author { get; init; }

    [JsonPropertyName("direct_message")]
    public bool? DirectMessage { get; init; }

    [JsonPropertyName("src_guild_id")]
    public ulong? SrcGuildId { get; init; }
}

internal class MessageDeleteOperator
{
    [JsonPropertyName("id")]
    public required ulong Id { get; init; }
}
