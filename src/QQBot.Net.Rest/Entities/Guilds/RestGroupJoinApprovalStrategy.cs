using System.Diagnostics;
using Model = QQBot.API.JoinApprovalStrategy;
using MutationModel = QQBot.API.Rest.JoinApprovalStrategyMutationResponse;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的入群自动审批策略。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestGroupJoinApprovalStrategy : RestEntity<string>, IGroupJoinApprovalStrategy
{
    /// <inheritdoc />
    public IReadOnlyCollection<Guid> GroupIds { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<ulong> GroupNumbers { get; private set; }

    /// <inheritdoc />
    public int WhitelistUserCount { get; private set; }

    /// <inheritdoc />
    public bool IsEnabled { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset ExpiresAt { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <inheritdoc />
    public string? Remark { get; private set; }

    internal RestGroupJoinApprovalStrategy(BaseQQBotClient client, string id)
        : base(client, id)
    {
        GroupIds = [];
        GroupNumbers = [];
    }

    internal static RestGroupJoinApprovalStrategy Create(BaseQQBotClient client, Model model)
    {
        RestGroupJoinApprovalStrategy entity = new(client, model.StrategyId);
        entity.Update(model);
        return entity;
    }

    internal static RestGroupJoinApprovalStrategy CreateFromMutation(BaseQQBotClient client, MutationModel model)
    {
        RestGroupJoinApprovalStrategy entity = new(client, model.StrategyId ?? string.Empty);
        entity.IsEnabled = string.Equals(model.IsEnable, "on", StringComparison.OrdinalIgnoreCase);
        if (model.ExpireAt.HasValue)
            entity.ExpiresAt = model.ExpireAt.Value;
        return entity;
    }

    internal void Update(Model model)
    {
        GroupIds = StrategyHelper.ParseGroupOpenids(model.GroupOpenids);
        GroupNumbers = model.GroupIds ?? [];
        WhitelistUserCount = model.WhitelistUserCount;
        IsEnabled = string.Equals(model.IsEnable, "on", StringComparison.OrdinalIgnoreCase);
        ExpiresAt = model.ExpireAt;
        CreatedAt = model.CreatedAt;
        UpdatedAt = model.UpdatedAt;
        Remark = model.Remark;
    }

    /// <inheritdoc />
    public Task ModifyAsync(Action<ModifyGroupJoinApprovalStrategyProperties> func, RequestOptions? options = null) =>
        StrategyHelper.ModifyStrategyAsync(Client, Id, func, options);

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) =>
        StrategyHelper.DeleteStrategyAsync(Client, Id, options);

    /// <inheritdoc />
    public Task ExecuteAsync(RequestOptions? options = null) =>
        StrategyHelper.ExecuteStrategyAsync(Client, Id, options);

    /// <inheritdoc />
    public Task AddWhitelistAsync(IEnumerable<string> qqNumbers, RequestOptions? options = null) =>
        StrategyHelper.AddWhitelistAsync(Client, Id, qqNumbers, options);

    /// <inheritdoc />
    public Task RemoveWhitelistAsync(IEnumerable<string> qqNumbers, RequestOptions? options = null) =>
        StrategyHelper.RemoveWhitelistAsync(Client, Id, qqNumbers, options);

    private string DebuggerDisplay => $"{Id} ({(IsEnabled ? "Enabled" : "Disabled")}, JoinApprovalStrategy)";
}
