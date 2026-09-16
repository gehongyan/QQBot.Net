using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetMenuResponse
{
    [JsonPropertyName("version")]
    public int Version { get; init; }

    [JsonPropertyName("menu")]
    public Menu? Menu { get; init; }
}
