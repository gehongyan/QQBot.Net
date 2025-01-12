using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class UserBotEvent
{
    [JsonPropertyName("openid")]
    public required Guid OpenId { get; init; }

    [JsonPropertyName("timestamp")]
    public required int Timestamp { get; init; }
}
