using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class RespondInteractionParams
{
    [JsonPropertyName("code")]
    public required InteractionResponseCode Code { get; init; }
}
