using System.Collections.Concurrent;

namespace QQBot.WebSocket;

internal class ClientState
{
    private const double CollectionMultiplier = 1.05; //Add 5% buffer to handle growth

    private readonly ConcurrentDictionary<ulong, SocketGuildChannel> _channels;
    private readonly ConcurrentDictionary<ulong, SocketDMChannel> _dmChannels;
    private readonly ConcurrentDictionary<Guid, SocketGroupChannel> _groupChannels;
    private readonly ConcurrentDictionary<Guid, SocketUserChannel> _userChannels;
    private readonly ConcurrentDictionary<ulong, SocketGuild> _guilds;
    private readonly ConcurrentDictionary<string, SocketGlobalUser> _users;

    internal IReadOnlyCollection<SocketChannel> Channels => _channels.ToReadOnlyCollection();

    internal IReadOnlyCollection<SocketDMChannel> DMChannels => _dmChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGroupChannel> GroupChannels => _groupChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketUserChannel> UserChannels => _userChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGuild> Guilds => _guilds.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGlobalUser> Users => _users.ToReadOnlyCollection();

    public ClientState(int guildCount)
    {
        _channels = new ConcurrentDictionary<ulong, SocketGuildChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _dmChannels = new ConcurrentDictionary<ulong, SocketDMChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _groupChannels = new ConcurrentDictionary<Guid, SocketGroupChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _userChannels = new ConcurrentDictionary<Guid, SocketUserChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _guilds = new ConcurrentDictionary<ulong, SocketGuild>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(guildCount * CollectionMultiplier));
        _users = new ConcurrentDictionary<string, SocketGlobalUser>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
    }

    #region Guild

    internal SocketGuild? GetGuild(ulong id) => _guilds.GetValueOrDefault(id);

    internal void AddGuild(SocketGuild guild) => _guilds[guild.Id] = guild;

    #endregion

    #region GuildChannel

    internal SocketGuildChannel? GetGuildChannel(ulong channelId) => _channels.GetValueOrDefault(channelId);

    internal void AddGuildChannel(SocketGuildChannel channel) => _channels[channel.Id] = channel;

    #endregion

    #region UserChannel

    internal SocketUserChannel? GetUserChannel(Guid id) => _userChannels.GetValueOrDefault(id);

    internal void AddUserChannel(SocketUserChannel channel) => _userChannels[channel.Id] = channel;

    internal SocketUserChannel GetOrAddUserChannel(Guid id, Func<Guid, SocketUserChannel> channelFactory) =>
        _userChannels.GetOrAdd(id, channelFactory);

    #endregion

    #region DMChannel

    internal SocketDMChannel GetOrAddDMChannel(ulong id, Func<ulong, SocketDMChannel> channelFactory) =>
        _dmChannels.GetOrAdd(id, channelFactory);

    #endregion

    #region GroupChannel

    internal SocketGroupChannel GetOrAddGroupChannel(Guid id, Func<Guid, SocketGroupChannel> channelFactory) =>
        _groupChannels.GetOrAdd(id, channelFactory);

    #endregion

    #region Users

    internal SocketGlobalUser GetOrAddUser(ulong id, Func<ulong, SocketGlobalUser> userFactory) =>
        _users.GetOrAdd(id.ToString(), _ => userFactory(id));

    internal SocketGlobalUser GetOrAddUser(Guid id, Func<Guid, SocketGlobalUser> userFactory) =>
        _users.GetOrAdd(id.ToIdString(), _ => userFactory(id));

    internal SocketGlobalUser? RemoveUser(string id) =>
        _users.TryRemove(id, out SocketGlobalUser? user) ? user : null;


    #endregion
}
