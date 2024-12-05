using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.Rest;

/// <summary>
///     表示频道中的一个基于 REST 的日程子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestScheduleChannel : RestGuildChannel, IScheduleChannel
{
    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public PrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    internal RestScheduleChannel(BaseQQBotClient client, ulong id, IGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Schedule;
    }

    internal static new RestScheduleChannel Create(BaseQQBotClient client, IGuild guild, Model model)
    {
        RestScheduleChannel entity = new(client, model.Id, guild);
        entity.Update(model);
        return entity;
    }

    internal override void Update(Model model)
    {
        base.Update(model);
        CategoryId = model.ParentId;
        PrivateType = model.PrivateType;
        SpeakPermission = model.SpeakPermission;
        Permission = model.Permissions is not null ? Enum.Parse<ChannelPermission>(model.Permissions) : null; // TODO
    }

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyScheduleChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(model);
    }

    /// <inheritdoc />
    public Task<ChannelPermissions> GetPermissionsAsync(IGuildMember user, RequestOptions? options = null) =>
        ChannelHelper.GetPermissionsAsync(this, Client, user, options);

    /// <inheritdoc />
    public Task<ChannelPermissions> GetPermissionsAsync(IRole role, RequestOptions? options = null) =>
        ChannelHelper.GetPermissionsAsync(this, Client, role, options);

    /// <inheritdoc />
    public Task ModifyPermissionsAsync(IGuildMember user, OverwritePermissions permissions, RequestOptions? options = null) =>
        ChannelHelper.ModifyPermissionsAsync(this, Client, user, permissions, options);

    /// <inheritdoc />
    public Task ModifyPermissionsAsync(IRole role, OverwritePermissions permissions, RequestOptions? options = null) =>
        ChannelHelper.ModifyPermissionsAsync(this, Client, role, permissions, options);

    private string DebuggerDisplay => $"{Name} ({Id}, Schedule)";
}
