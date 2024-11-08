using System.Diagnostics;
using QQBot.Rest;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示频道中的一个基于网关的具有语音聊天能力的子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketVoiceChannel : SocketGuildChannel, IVoiceChannel
{
    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public PrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    internal SocketVoiceChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Voice;
    }

    internal static new SocketVoiceChannel Create(SocketGuild guild, ClientState state, Model model)
    {
        SocketVoiceChannel entity = new(guild.Client, model.Id, guild);
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
    public async Task ModifyAsync(Action<ModifyVoiceChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(Client.State, model);
    }

    private string DebuggerDisplay => $"{Name} ({Id}, Voice)";
}
