using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API;

internal class MenuItem
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(MenuItemTypeJsonConverter))]
    public MenuItemType Type { get; init; }

    [JsonPropertyName("sub_menu_items")]
    public SubMenuItem[]? SubMenuItems { get; init; }

    [JsonPropertyName("send_message")]
    public string? SendMessage { get; init; }

    [JsonPropertyName("link")]
    public string? Link { get; init; }

    [JsonPropertyName("switch")]
    public MenuSwitch? Switch { get; init; }
}
