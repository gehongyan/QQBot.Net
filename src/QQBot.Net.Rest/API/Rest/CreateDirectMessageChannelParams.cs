using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateDirectMessageChannelParams
{
    [JsonPropertyName("recipient_id")]
    public required string RecipientId { get; init; }

    [JsonPropertyName("source_guild_id")]
    public required string SourceGuildId { get; init; }
}
