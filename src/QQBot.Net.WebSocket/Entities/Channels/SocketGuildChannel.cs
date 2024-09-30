using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道内的子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGuildChannel : SocketChannel, IGuildChannel
{
    /// <inheritdoc />
    public new ulong Id { get; }

    /// <inheritdoc cref="QQBot.IGuildChannel.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public ChannelType Type { get; internal set; }

    /// <inheritdoc />
    public int Position { get; private set; }

    /// <inheritdoc />
    public ulong? CreatorId { get; private set; }

    internal SocketGuildChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id.ToString())
    {
        Id = id;
        Name = string.Empty;
        Guild = guild;
        Type = ChannelType.Unspecified;
    }

    internal static SocketGuildChannel Create(SocketGuild guild, ClientState state, Model model) =>
        model.Type switch
        {
            ChannelType.Category => SocketCategoryChannel.Create(guild, state, model),
            ChannelType.Text => SocketTextChannel.Create(guild, state, model),
            ChannelType.Voice => SocketVoiceChannel.Create(guild, state, model),
            ChannelType.LiveStream => SocketLiveStreamChannel.Create(guild, state, model),
            ChannelType.Application => SocketApplicationChannel.Create(guild, state, model),
            ChannelType.Forum => SocketForumChannel.Create(guild, state, model),
            _ => new SocketGuildChannel(guild.Client, model.Id, guild)
        };

    internal virtual void Update(ClientState state, Model model)
    {
        Name = model.Name;
        Position = model.Position;
        CreatorId = model.OwnerId;
    }

    /// <inheritdoc />
    public virtual Task UpdateAsync(RequestOptions? options = null) =>
        SocketChannelHelper.UpdateAsync(this, options);

    /// <inheritdoc cref="QQBot.WebSocket.SocketGuildChannel.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id}, Guild)";

    #region IGuildChannel

    /// <inheritdoc />
    IGuild IGuildChannel.Guild => Guild;

    /// <inheritdoc />
    ulong IGuildChannel.GuildId => Guild.Id;

    #endregion
}
