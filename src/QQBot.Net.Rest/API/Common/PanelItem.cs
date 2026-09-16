using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class PanelItem
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("desc")]
    public string? Desc { get; init; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(CommandPanelItemTypeJsonConverter))]
    public CommandPanelItemType Type { get; init; }

    [JsonPropertyName("only_admin")]
    public bool OnlyAdmin { get; init; }

    [JsonPropertyName("link")]
    public string? Link { get; init; }
}
