using System.Diagnostics;
using QQBot.Rest;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示频道中的一个基于网关的论坛子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketForumChannel : SocketGuildChannel, IForumChannel
{
    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public PrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    internal SocketForumChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Forum;
    }

    internal static new SocketForumChannel Create(SocketGuild guild, ClientState state, Model model)
    {
        SocketForumChannel entity = new(guild.Client, model.Id, guild);
        entity.Update(state, model);
        return entity;
    }

    internal override void Update(ClientState state, Model model)
    {
        base.Update(state, model);
        CategoryId = model.ParentId;
        PrivateType = model.PrivateType;
        SpeakPermission = model.SpeakPermission;
        Permission = model.Permissions is not null ? Enum.Parse<ChannelPermission>(model.Permissions) : null; // TODO
    }

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyForumChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(Client.State, model);
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

    /// <inheritdoc cref="QQBot.IForumChannel.GetThreadsAsync(QQBot.RequestOptions)" />
    public async Task<IReadOnlyCollection<RestThread>> GetThreadsAsync(RequestOptions? options = null) =>
        await ChannelHelper.GetThreadsAsync(this, Client, options).ConfigureAwait(false);

    /// <inheritdoc cref="QQBot.IForumChannel.GetThreadAsync(System.String,QQBot.RequestOptions)" />
    public async Task<RestThread> GetThreadAsync(string id, RequestOptions? options = null)
    {
        API.Thread model = await ChannelHelper.GetThreadAsync(this, Client, id, options).ConfigureAwait(false);
        return RestThread.Create(Client, this, model.AuthorId, model.ThreadInfo);
    }

    /// <inheritdoc />
    public Task CreateThreadAsync(string title, ThreadTextType textType, string content, RequestOptions? options = null) =>
        ChannelHelper.CreateThreadAsync(this, Client, title, textType, content, options);

    /// <inheritdoc />
    public Task CreateThreadAsync(string title, RichTextBuilder content, RequestOptions? options = null) =>
        ChannelHelper.CreateThreadAsync(this, Client, title, content, options);

    private string DebuggerDisplay => $"{Name} ({Id}, Forum)";

    #region IForumChannel

    /// <inheritdoc />
    async Task<IReadOnlyCollection<IThread>> IForumChannel.GetThreadsAsync(RequestOptions? options) =>
        await GetThreadsAsync(options);

    /// <inheritdoc />
    async Task<IThread> IForumChannel.GetThreadAsync(string id, RequestOptions? options) =>
        await GetThreadAsync(id, options);

    #endregion
}
