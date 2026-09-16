using System.Diagnostics;
using Model = QQBot.API.PanelRecord;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的指令面板。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestCommandPanel : RestEntity<string>, ICommandPanel
{
    /// <inheritdoc />
    public CommandPanelScope Scope { get; private set; }

    /// <inheritdoc />
    public CommandPanelTargetType TargetType { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<CommandPanelItem> Items { get; private set; }

    /// <inheritdoc />
    public string? Remark { get; private set; }

    /// <inheritdoc />
    public int Version { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; private set; }

    internal RestCommandPanel(BaseQQBotClient client, string id)
        : base(client, id)
    {
        Items = [];
    }

    internal static RestCommandPanel Create(BaseQQBotClient client, Model model)
    {
        RestCommandPanel entity = new(client, model.PanelId);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        Scope = model.Scope;
        TargetType = model.TargetType;
        Items = PanelHelper.ToItems(model.Panel?.Items);
        Remark = model.Panel?.Remark;
        Version = model.Version;
        CreatedAt = model.CreatedAt;
        UpdatedAt = model.UpdatedAt;
    }

    /// <inheritdoc />
    public Task ModifyAsync(IEnumerable<CommandPanelItem> items,
        Action<CommandPanelProperties>? func = null, RequestOptions? options = null) =>
        PanelHelper.ModifyPanelAsync(Client, Id, items, func, options);

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) =>
        PanelHelper.DeletePanelAsync(Client, Id, options);

    /// <inheritdoc />
    public Task AddTargetsAsync(IEnumerable<Guid> targetIds, RequestOptions? options = null) =>
        PanelHelper.ModifyPanelTargetAsync(Client, Id, Scope, "add", targetIds, options);

    /// <inheritdoc />
    public Task RemoveTargetsAsync(IEnumerable<Guid> targetIds, RequestOptions? options = null) =>
        PanelHelper.ModifyPanelTargetAsync(Client, Id, Scope, "del", targetIds, options);

    private string DebuggerDisplay => $"{Id} ({Scope}/{TargetType}, CommandPanel)";
}
