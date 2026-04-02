using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class UserBotEvent
{
    [GuidJsonConverter]
    [JsonPropertyName("openid")]
    public required Guid OpenId { get; init; }

    [JsonPropertyName("timestamp")]
    public required int Timestamp { get; init; }
}
