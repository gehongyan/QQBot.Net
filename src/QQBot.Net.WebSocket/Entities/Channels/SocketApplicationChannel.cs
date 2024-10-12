using System.Diagnostics;
using QQBot.API;

namespace QQBot.WebSocket;

/// <summary>
///     表示频道中的一个基于网关的直播子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketApplicationChannel : SocketGuildChannel, IApplicationChannel
{
    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public ChannelPrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    /// <inheritdoc />
    public ChannelApplication? ApplicationType { get; private set; }

    internal SocketApplicationChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Application;
    }

    internal static new SocketApplicationChannel Create(SocketGuild guild, ClientState state, Channel model)
    {
        SocketApplicationChannel entity = new(guild.Client, model.Id, guild);
        entity.Update(state, model);
        return entity;
    }

    internal override void Update(ClientState state, Channel model)
    {
        base.Update(state, model);
        CategoryId = model.ParentId;
        PrivateType = model.PrivateType;
        SpeakPermission = model.SpeakPermission;
        Permission = model.Permissions is not null ? Enum.Parse<ChannelPermission>(model.Permissions) : null; // TODO
        ApplicationType = model.ApplicationId;
    }

    private string DebuggerDisplay => $"{Name} ({Id}, Application)";
}
