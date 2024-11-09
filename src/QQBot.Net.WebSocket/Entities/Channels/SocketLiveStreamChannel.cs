using System.Diagnostics;
using QQBot.Rest;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示频道中的一个基于网关的直播子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketLiveStreamChannel : SocketGuildChannel, ILiveStreamChannel
{
    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public PrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    internal SocketLiveStreamChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.LiveStream;
    }

    internal static new SocketLiveStreamChannel Create(SocketGuild guild, ClientState state, Model model)
    {
        SocketLiveStreamChannel entity = new(guild.Client, model.Id, guild);
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
    public async Task ModifyAsync(Action<ModifyLiveStreamChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(Client.State, model);
    }

    /// <inheritdoc />
    public async Task<int> CountOnlineUsersAsync(RequestOptions? options = null) =>
        await ChannelHelper.CountOnlineUsersAsync(this, Client, options).ConfigureAwait(false);

    private string DebuggerDisplay => $"{Name} ({Id}, LiveStream)";
}
