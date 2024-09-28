using System.Collections.Concurrent;
using Model = QQBot.API.Guild;
using ChannelModel = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道。
/// </summary>
public class SocketGuild : SocketEntity<ulong>, IGuild, IUpdateable
{
    private readonly ConcurrentDictionary<ulong, SocketGuildChannel> _channels;

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public ulong OwnerId { get; private set; }

    /// <inheritdoc />
    public bool IsOwner { get; private set; }

    /// <inheritdoc />
    public int MemberCount { get; private set; }

    /// <inheritdoc />
    public int MaxMembers { get; private set; }

    /// <inheritdoc />
    public string Description { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset JoinedAt { get; private set; }

    /// <inheritdoc />
    public bool IsAvailable { get; private set; }

    /// <summary>
    ///     获取此服务器是否已连接至网关。
    /// </summary>
    public bool IsConnected { get; internal set; }

    /// <summary>
    ///     获取此服务器中所有具有文字聊天能力的频道。
    /// </summary>
    /// <remarks>
    ///     语音频道也是一种文字频道，此属性本意用于获取所有具有文字聊天能力的频道，通过此方法获取到的文字频道列表中也包含了语音频道。
    ///     如需获取频道的实际类型，请参考 <see cref="QQBot.ChannelExtensions.GetChannelType(QQBot.IChannel)"/>。
    /// </remarks>
    public IReadOnlyCollection<SocketTextChannel> TextChannels => [..Channels.OfType<SocketTextChannel>()];

    /// <summary>
    ///     获取此服务器中所有具有语音聊天能力的频道。
    /// </summary>
    public IReadOnlyCollection<SocketVoiceChannel> VoiceChannels => [..Channels.OfType<SocketVoiceChannel>()];

    /// <summary>
    ///     获取此服务器中的所有分组频道。
    /// </summary>
    public IReadOnlyCollection<SocketCategoryChannel> CategoryChannels => [..Channels.OfType<SocketCategoryChannel>()];

    /// <summary>
    ///     获取此服务器的所有频道。
    /// </summary>
    public IReadOnlyCollection<SocketGuildChannel> Channels => [.._channels.Values];

    /// <inheritdoc />
    internal SocketGuild(QQBotSocketClient client, ulong id)
        : base(client, id)
    {
        Name = string.Empty;
        Description = string.Empty;
        _channels = [];
    }

    internal static SocketGuild Create(QQBotSocketClient client, ClientState state, Model model)
    {
        SocketGuild entity = new(client, model.Id);
        entity.Update(state, model);
        return entity;
    }

    internal void Update(ClientState state, Model model)
    {
        Name = model.Name;
        OwnerId = model.OwnerId;
        IsOwner = model.OwnerId == Client.CurrentUser?.Id;
        MemberCount = model.MemberCount;
        MaxMembers = model.MaxMembers;
        Description = model.Description;
        JoinedAt = model.JoinedAt;

        // Only when both roles and channels are not null will the guild be considered available.
        IsAvailable = false; // TODO: model.Roles is not null && model.Channels is not null;
    }

    internal void Update(ClientState state, IReadOnlyCollection<ChannelModel> models)
    {
        foreach (ChannelModel model in models)
        {
            if (_channels.TryGetValue(model.Id, out SocketGuildChannel? existing))
            {
                existing.Update(state, model);
                continue;
            }
            SocketGuildChannel channel = SocketGuildChannel.Create(this, state, model);
            state.AddChannel(channel);
            _channels.TryAdd(channel.Id, channel);
        }
    }

    /// <inheritdoc />
    public Task UpdateAsync(RequestOptions? options = null) => SocketGuildHelper.UpdateAsync(this, Client, options);
}
