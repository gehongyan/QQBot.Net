using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class StrategyHelper
{
    public static IAsyncEnumerable<IReadOnlyCollection<IGroupJoinApprovalStrategy>> GetStrategiesAsync(
        BaseQQBotClient client, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<IGroupJoinApprovalStrategy>(
            QQBotConfig.MaxJoinApprovalStrategiesPerBatch,
            async (info, _) =>
            {
                GetJoinApprovalStrategyListResponse response = await client.ApiClient
                    .GetJoinApprovalStrategyListAsync(info.Cookie,
                        QQBotConfig.MaxJoinApprovalStrategiesPerBatch, options).ConfigureAwait(false);
                info.Cookie = response.NextCursor;
                return response.Strategies
                    .Select(x => (IGroupJoinApprovalStrategy)RestGroupJoinApprovalStrategy.Create(client, x))
                    .ToArray();
            },
            nextPage: (info, _) => !string.IsNullOrEmpty(info.Cookie));
    }

    public static Task<RestGroupJoinApprovalStrategy> CreateStrategyAsync(BaseQQBotClient client,
        IEnumerable<Guid> groupIds, Action<GroupJoinApprovalStrategyProperties>? func, RequestOptions? options) =>
        CreateStrategyCoreAsync(client, groupIds.Select(x => x.ToIdString()).ToArray(), null, func, options);

    public static Task<RestGroupJoinApprovalStrategy> CreateStrategyAsync(BaseQQBotClient client,
        IEnumerable<ulong> groupNumbers, Action<GroupJoinApprovalStrategyProperties>? func, RequestOptions? options) =>
        CreateStrategyCoreAsync(client, null, groupNumbers.ToArray(), func, options);

    private static async Task<RestGroupJoinApprovalStrategy> CreateStrategyCoreAsync(BaseQQBotClient client,
        string[]? groupOpenids, ulong[]? groupIds, Action<GroupJoinApprovalStrategyProperties>? func,
        RequestOptions? options)
    {
        GroupJoinApprovalStrategyProperties props = new();
        func?.Invoke(props);

        CreateJoinApprovalStrategyParams args = new()
        {
            GroupOpenids = groupOpenids,
            GroupIds = groupIds,
            IsEnable = props.IsEnabled ? "on" : "off",
            ExpireAt = props.ExpiresAt?.ToString("yyyy-MM-ddTHH:mm:ssK"),
            Remark = props.Remark
        };
        JoinApprovalStrategyMutationResponse response = await client.ApiClient
            .CreateJoinApprovalStrategyAsync(args, options).ConfigureAwait(false);

        // 创建接口仅返回 strategy_id/is_enable/expire_at；回读完整策略以填充实体。
        return await GetStrategyByIdAsync(client, response.StrategyId ?? string.Empty, options).ConfigureAwait(false)
            ?? RestGroupJoinApprovalStrategy.CreateFromMutation(client, response);
    }

    private static async Task<RestGroupJoinApprovalStrategy?> GetStrategyByIdAsync(BaseQQBotClient client,
        string strategyId, RequestOptions? options)
    {
        if (string.IsNullOrEmpty(strategyId))
            return null;
        await foreach (IReadOnlyCollection<IGroupJoinApprovalStrategy> page in GetStrategiesAsync(client, options).ConfigureAwait(false))
            foreach (IGroupJoinApprovalStrategy strategy in page)
                if (strategy is RestGroupJoinApprovalStrategy rest && rest.Id == strategyId)
                    return rest;
        return null;
    }

    public static Task ModifyStrategyAsync(BaseQQBotClient client, string strategyId,
        Action<ModifyGroupJoinApprovalStrategyProperties> func, RequestOptions? options)
    {
        ModifyGroupJoinApprovalStrategyProperties props = new();
        func(props);
        ModifyJoinApprovalStrategyParams args = new()
        {
            IsEnable = props.IsEnabled.HasValue ? (props.IsEnabled.Value ? "on" : "off") : null,
            ExpireAt = props.ExpiresAt?.ToString("yyyy-MM-ddTHH:mm:ssK"),
            Remark = props.Remark,
            GroupAction = ToGroupAction(props.AddGroups, "add") ?? ToGroupAction(props.RemoveGroups, "del")
        };
        return client.ApiClient.ModifyJoinApprovalStrategyAsync(strategyId, args, options);
    }

    private static JoinApprovalStrategyGroupAction? ToGroupAction(
        GroupJoinApprovalStrategyGroupAction? action, string op)
    {
        if (action is null)
            return null;
        return new JoinApprovalStrategyGroupAction
        {
            Op = op,
            GroupOpenids = action.GroupIds?.Select(x => x.ToIdString()).ToArray(),
            GroupIds = action.GroupNumbers?.ToArray()
        };
    }

    public static Task DeleteStrategyAsync(BaseQQBotClient client, string strategyId, RequestOptions? options) =>
        client.ApiClient.DeleteJoinApprovalStrategyAsync(strategyId, options);

    public static Task ExecuteStrategyAsync(BaseQQBotClient client, string strategyId, RequestOptions? options) =>
        client.ApiClient.ExecuteJoinApprovalStrategyAsync(strategyId, options);

    public static Task AddWhitelistAsync(BaseQQBotClient client, string strategyId,
        IEnumerable<string> qqNumbers, RequestOptions? options)
    {
        OperateJoinApprovalWhitelistParams args = new()
        {
            Op = "add",
            WhitelistUsers = qqNumbers.ToArray()
        };
        return client.ApiClient.OperateJoinApprovalWhitelistAsync(strategyId, args, options);
    }

    public static Task RemoveWhitelistAsync(BaseQQBotClient client, string strategyId,
        IEnumerable<string> qqNumbers, RequestOptions? options)
    {
        OperateJoinApprovalWhitelistParams args = new()
        {
            Op = "del",
            WhitelistUsers = qqNumbers.ToArray()
        };
        return client.ApiClient.OperateJoinApprovalWhitelistAsync(strategyId, args, options);
    }

    internal static IReadOnlyCollection<Guid> ParseGroupOpenids(string[]? openIds) =>
        openIds is null
            ? []
            : openIds.Select(x => Guid.TryParse(x, out Guid g) ? g : (Guid?)null)
                .Where(x => x.HasValue).Select(x => x!.Value).ToArray();
}
