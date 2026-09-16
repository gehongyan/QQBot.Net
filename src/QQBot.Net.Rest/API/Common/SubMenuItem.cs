using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class SubMenuItem
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(SubMenuItemTypeJsonConverter))]
    public SubMenuItemType Type { get; init; }

    [JsonPropertyName("send_message")]
    public string? SendMessage { get; init; }

    [JsonPropertyName("link")]
    public string? Link { get; init; }
}
