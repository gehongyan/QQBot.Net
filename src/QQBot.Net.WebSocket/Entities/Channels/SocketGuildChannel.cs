﻿using System.Diagnostics;
using QQBot.Rest;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的子频道内的子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGuildChannel : SocketChannel, IGuildChannel
{
    /// <inheritdoc cref="QQBot.IGuildChannel.Id" />
    public new ulong Id { get; }

    /// <inheritdoc cref="QQBot.IGuildChannel.Guild" />
    public SocketGuild Guild { get; }

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public ChannelType Type { get; internal set; }

    /// <inheritdoc />
    public int? Position { get; private set; }

    /// <inheritdoc />
    public ulong? CreatorId { get; private set; }

    internal SocketGuildChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id.ToIdString())
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
            ChannelType.Schedule => SocketScheduleChannel.Create(guild, state, model),
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

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyGuildChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(Client.State, model);
    }

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) => ChannelHelper.DeleteAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.WebSocket.SocketGuildChannel.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id}, Guild)";

    internal SocketGuildChannel Clone() => (SocketGuildChannel)MemberwiseClone();

    #region Users

    /// <inheritdoc cref="QQBot.WebSocket.SocketChannel.GetUser(System.String)" />
    public SocketGuildMember? GetUser(ulong id) => null; // Override in derived classes

    /// <inheritdoc />
    protected override SocketUser? GetUserInternal(string id)
    {
        if (!ulong.TryParse(id, out ulong userId)) return null;
        return GetUser(userId);
    }

    #endregion

    #region IChannel

    /// <inheritdoc />
    Task<IUser?> IChannel.GetUserAsync(string id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(ulong.TryParse(id, out ulong userId) ? GetUserInternal(userId.ToIdString()) : null);

    #endregion

    #region IGuildChannel

    /// <inheritdoc />
    IGuild IGuildChannel.Guild => Guild;

    /// <inheritdoc />
    ulong IGuildChannel.GuildId => Guild.Id;

    /// <inheritdoc />
    Task<IGuildUser?> IGuildChannel.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IGuildUser?>(GetUser(id));

    #endregion
}
