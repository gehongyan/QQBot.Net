using System.Text.Json.Serialization;

namespace QQBot.API;

internal class Menu
{
    [JsonPropertyName("items")]
    public MenuItem[]? Items { get; init; }
}
