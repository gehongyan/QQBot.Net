using System.Collections.Concurrent;
using QQBot.Rest;
using Model = QQBot.API.Guild;
using ChannelModel = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道。
/// </summary>
public class SocketGuild : SocketEntity<ulong>, IGuild, IUpdateable
{
    private readonly ConcurrentDictionary<ulong, SocketGuildChannel> _channels;
    private readonly ConcurrentDictionary<ulong, SocketGuildMember> _members;

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

    /// <summary>
    ///     获取此服务器内已缓存的成员数量。
    /// </summary>
    public int DownloadedMemberCount => _members.Count;

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

    /// <summary>
    ///     获取当前登录的用户。
    /// </summary>
    public SocketGuildMember? CurrentUser =>
        Client.CurrentUser is { Id: var id } && _members.TryGetValue(id, out SocketGuildMember? member) ? member : null;

    /// <inheritdoc />
    internal SocketGuild(QQBotSocketClient client, ulong id)
        : base(client, id)
    {
        Name = string.Empty;
        Description = string.Empty;
        _channels = [];
        _members = [];
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
            state.AddGuildChannel(channel);
            _channels.TryAdd(channel.Id, channel);
        }
    }

    /// <inheritdoc />
    public Task UpdateAsync(RequestOptions? options = null) => SocketGuildHelper.UpdateAsync(this, Client, options);

    #region Users

    /// <summary>
    ///     获取此频道内的用户。
    /// </summary>
    /// <remarks>
    ///     此方法可能返回 <c>null</c>，因为在大型频道中，用户列表的缓存可能不完整。
    /// </remarks>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketGuildMember? GetUser(ulong id) => _members.GetValueOrDefault(id);

    internal SocketGuildMember AddOrUpdateUser(API.User userModel, API.Member? memberModel)
    {
        if (_members.TryGetValue(userModel.Id, out SocketGuildMember? cachedMember))
        {
            cachedMember.Update(Client.State, userModel, memberModel);
            return cachedMember;
        }

        SocketGuildMember member = SocketGuildMember.Create(this, Client.State, userModel, memberModel);
        member.GlobalUser.AddRef();
        _members[member.Id] = member;
        return member;
    }

    #endregion

    #region Channels

    /// <summary>
    ///     获取此服务器内的频道。
    /// </summary>
    /// <param name="id"> 要获取的频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketGuildChannel? GetChannel(ulong id) => Client.State.GetGuildChannel(id);

    /// <summary>
    ///     获取此频道中所有具有文字聊天能力的子频道。
    /// </summary>
    /// <param name="id"> 要获取的子频道的 ID。 </param>
    /// <returns> 与指定的 <paramref name="id"/> 关联的子频道；如果未找到，则返回 <c>null</c>。 </returns>
    public SocketTextChannel? GetTextChannel(ulong id) => GetChannel(id) as SocketTextChannel;

    #endregion

    #region IGuild

    /// <inheritdoc />
    async Task<IGuildMember?> IGuild.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options)
    {
        IGuildMember? user = GetUser(id);
        if (user is not null || mode == CacheMode.CacheOnly)
            return user;
        return await GuildHelper.GetUserAsync(this, Client, id, options).ConfigureAwait(false);
    }

    #endregion
}
