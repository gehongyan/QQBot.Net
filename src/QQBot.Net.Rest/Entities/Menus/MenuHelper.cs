using QQBot.API.Rest;
using ApiMenu = QQBot.API.Menu;
using ApiMenuItem = QQBot.API.MenuItem;
using ApiSubMenuItem = QQBot.API.SubMenuItem;
using ApiMenuSwitch = QQBot.API.MenuSwitch;

namespace QQBot.Rest;

internal static class MenuHelper
{
    public static async Task<BotMenu> GetMenuAsync(BaseQQBotClient client, RequestOptions? options)
    {
        GetMenuResponse response = await client.ApiClient.GetMenuAsync(options).ConfigureAwait(false);
        return new BotMenu(response.Version, ToMenuItems(response.Menu?.Items));
    }

    public static async Task<BotMenu> ModifyMenuAsync(BaseQQBotClient client,
        IEnumerable<MenuItem> items, RequestOptions? options)
    {
        ModifyMenuParams args = new()
        {
            Menu = new ApiMenu { Items = items.Select(ToApiMenuItem).ToArray() }
        };
        ModifyMenuResponse response = await client.ApiClient.ModifyMenuAsync(args, options).ConfigureAwait(false);
        return new BotMenu(response.Version, items.ToArray());
    }

    #region Wire -> Public

    private static IReadOnlyCollection<MenuItem> ToMenuItems(ApiMenuItem[]? items) =>
        items is null ? [] : items.Select(ToMenuItem).ToArray();

    private static MenuItem ToMenuItem(ApiMenuItem model)
    {
        string name = model.Name ?? string.Empty;
        return model.Type switch
        {
            MenuItemType.Switch => MenuItem.CreateSwitch(name,
                model.Switch?.SwitchId ?? string.Empty, model.Switch?.Default ?? false),
            MenuItemType.Link => MenuItem.CreateLink(name, model.Link ?? string.Empty),
            MenuItemType.Menu => MenuItem.CreateSubMenu(name,
                (model.SubMenuItems ?? []).Select(ToSubMenuItem)),
            _ => MenuItem.CreateSendMessage(name, model.SendMessage ?? string.Empty)
        };
    }

    private static SubMenuItem ToSubMenuItem(ApiSubMenuItem model)
    {
        string name = model.Name ?? string.Empty;
        return model.Type switch
        {
            SubMenuItemType.Link => SubMenuItem.CreateLink(name, model.Link ?? string.Empty),
            _ => SubMenuItem.CreateSendMessage(name, model.SendMessage ?? string.Empty)
        };
    }

    #endregion

    #region Public -> Wire

    private static ApiMenuItem ToApiMenuItem(MenuItem item) => new()
    {
        Name = item.Name,
        Type = item.Type,
        SendMessage = item.SendMessage,
        Link = item.Link,
        Switch = item.Switch is { } s ? new ApiMenuSwitch { SwitchId = s.SwitchId, Default = s.IsDefaultOn } : null,
        SubMenuItems = item.Type == MenuItemType.Menu
            ? item.SubMenuItems.Select(ToApiSubMenuItem).ToArray()
            : null
    };

    private static ApiSubMenuItem ToApiSubMenuItem(SubMenuItem item) => new()
    {
        Name = item.Name,
        Type = item.Type,
        SendMessage = item.SendMessage,
        Link = item.Link
    };

    #endregion
}
