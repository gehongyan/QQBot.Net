using QQBot.API.Rest;
using ApiPanel = QQBot.API.Panel;
using ApiPanelItem = QQBot.API.PanelItem;
using ApiPanelRecord = QQBot.API.PanelRecord;

namespace QQBot.Rest;

internal static class PanelHelper
{
    public static IAsyncEnumerable<IReadOnlyCollection<RestCommandPanel>> GetPanelsAsync(
        BaseQQBotClient client, CommandPanelScope scope, RequestOptions? options)
    {
        string scopeValue = ToScopeString(scope);
        return new PagedAsyncEnumerable<RestCommandPanel>(
            QQBotConfig.MaxCommandPanelsPerBatch,
            async (info, _) =>
            {
                GetPanelListResponse response = await client.ApiClient
                    .GetPanelListAsync(scopeValue, info.Cookie, QQBotConfig.MaxCommandPanelsPerBatch, options)
                    .ConfigureAwait(false);
                info.Cookie = response.NextCursor;
                return response.Records.Select(x => RestCommandPanel.Create(client, x)).ToArray();
            },
            nextPage: (info, _) => !string.IsNullOrEmpty(info.Cookie));
    }

    public static async Task<RestCommandPanel> GetPanelAsync(BaseQQBotClient client, string panelId, RequestOptions? options)
    {
        ApiPanelRecord model = await client.ApiClient.GetPanelAsync(panelId, options).ConfigureAwait(false);
        return RestCommandPanel.Create(client, model);
    }

    public static async Task<RestCommandPanel> CreatePanelAsync(BaseQQBotClient client, CommandPanelScope scope,
        IEnumerable<Guid>? targetIds, IEnumerable<CommandPanelItem> items,
        Action<CommandPanelProperties>? func, RequestOptions? options)
    {
        CommandPanelProperties props = new();
        func?.Invoke(props);

        bool specific = targetIds is not null;
        string[]? targets = targetIds?.Select(x => x.ToIdString()).ToArray();
        CreatePanelParams args = new()
        {
            Scope = scope,
            TargetType = specific ? CommandPanelTargetType.Specific : CommandPanelTargetType.All,
            UserOpenids = specific && scope == CommandPanelScope.C2C ? targets : null,
            GroupOpenids = specific && scope == CommandPanelScope.Group ? targets : null,
            Panel = ToApiPanel(items, props.Remark)
        };
        CreatePanelResponse response = await client.ApiClient.CreatePanelAsync(args, options).ConfigureAwait(false);

        // 创建接口仅返回 panel_id；回读详情以填充实体。
        return await GetPanelAsync(client, response.PanelId, options).ConfigureAwait(false);
    }

    public static Task ModifyPanelAsync(BaseQQBotClient client, string panelId,
        IEnumerable<CommandPanelItem> items, Action<CommandPanelProperties>? func, RequestOptions? options)
    {
        CommandPanelProperties props = new();
        func?.Invoke(props);
        ModifyPanelParams args = new() { Panel = ToApiPanel(items, props.Remark) };
        return client.ApiClient.ModifyPanelAsync(panelId, args, options);
    }

    public static Task DeletePanelAsync(BaseQQBotClient client, string panelId, RequestOptions? options) =>
        client.ApiClient.DeletePanelAsync(panelId, options);

    public static Task ModifyPanelTargetAsync(BaseQQBotClient client, string panelId, CommandPanelScope scope,
        string op, IEnumerable<Guid> targetIds, RequestOptions? options)
    {
        string[] targets = targetIds.Select(x => x.ToIdString()).ToArray();
        ModifyPanelTargetParams args = new()
        {
            Op = op,
            UserOpenids = scope == CommandPanelScope.C2C ? targets : null,
            GroupOpenids = scope == CommandPanelScope.Group ? targets : null
        };
        return client.ApiClient.ModifyPanelTargetAsync(panelId, args, options);
    }

    private static string ToScopeString(CommandPanelScope scope) => scope switch
    {
        CommandPanelScope.Group => "group",
        CommandPanelScope.Channel => "channel",
        CommandPanelScope.DM => "dm",
        _ => "c2c"
    };

    internal static IReadOnlyCollection<CommandPanelItem> ToItems(ApiPanelItem[]? items) =>
        items is null ? [] : items.Select(ToItem).ToArray();

    private static CommandPanelItem ToItem(ApiPanelItem model) => model.Type switch
    {
        CommandPanelItemType.Link => CommandPanelItem.CreateLink(
            model.Name ?? string.Empty, model.Link ?? string.Empty, model.Desc, model.OnlyAdmin),
        _ => CommandPanelItem.CreateCommand(model.Name ?? string.Empty, model.Desc, model.OnlyAdmin)
    };

    private static ApiPanel ToApiPanel(IEnumerable<CommandPanelItem> items, string? remark) => new()
    {
        Items = items.Select(ToApiItem).ToArray(),
        Remark = remark
    };

    private static ApiPanelItem ToApiItem(CommandPanelItem item) => new()
    {
        Name = item.Name,
        Desc = item.Description,
        Type = item.Type,
        OnlyAdmin = item.OnlyAdmin,
        Link = item.Link
    };
}
