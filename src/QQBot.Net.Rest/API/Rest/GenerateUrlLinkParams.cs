using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GenerateUrlLinkParams
{
    [JsonPropertyName("callback_data")]
    public string? CallbackData { get; init; }
}
