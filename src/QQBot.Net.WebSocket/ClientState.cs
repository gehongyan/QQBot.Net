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
    private readonly ConcurrentDictionary<string, SocketGlobalUser> _globalUsers;
    private readonly ConcurrentDictionary<ulong, SocketGuildUser> _guildUsers;

    internal IReadOnlyCollection<SocketChannel> Channels => _channels.ToReadOnlyCollection();

    internal IReadOnlyCollection<SocketDMChannel> DMChannels => _dmChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGroupChannel> GroupChannels => _groupChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketUserChannel> UserChannels => _userChannels.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGuild> Guilds => _guilds.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGlobalUser> GlobalUsers => _globalUsers.ToReadOnlyCollection();
    internal IReadOnlyCollection<SocketGuildUser> GuildUsers => _guildUsers.ToReadOnlyCollection();

    public ClientState(int guildCount)
    {
        _channels = new ConcurrentDictionary<ulong, SocketGuildChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _dmChannels = new ConcurrentDictionary<ulong, SocketDMChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _groupChannels = new ConcurrentDictionary<Guid, SocketGroupChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _userChannels = new ConcurrentDictionary<Guid, SocketUserChannel>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _guilds = new ConcurrentDictionary<ulong, SocketGuild>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(guildCount * CollectionMultiplier));
        _globalUsers = new ConcurrentDictionary<string, SocketGlobalUser>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
        _guildUsers = new ConcurrentDictionary<ulong, SocketGuildUser>(ConcurrentHashSet.DefaultConcurrencyLevel, 0);
    }

    #region Guild

    internal SocketGuild? GetGuild(ulong id) => _guilds.GetValueOrDefault(id);

    internal void AddGuild(SocketGuild guild) => _guilds[guild.Id] = guild;

    #endregion

    #region GuildChannel

    internal SocketGuildChannel? GetGuildChannel(ulong channelId) => _channels.GetValueOrDefault(channelId);

    internal void AddGuildChannel(SocketGuildChannel channel) => _channels[channel.Id] = channel;

    internal SocketGuildChannel? RemoveGuildChannel(ulong id) =>
        _channels.TryRemove(id, out SocketGuildChannel? channel) ? channel : null;

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

    #region Global Users

    internal SocketGlobalUser? GetGlobalUser(string id) => _globalUsers.GetValueOrDefault(id);

    internal SocketGlobalUser GetOrAddGlobalUser(ulong id, Func<ulong, SocketGlobalUser> userFactory) =>
        _globalUsers.GetOrAdd(id.ToIdString(), _ => userFactory(id));

    internal SocketGlobalUser GetOrAddGlobalUser(Guid id, Func<Guid, SocketGlobalUser> userFactory) =>
        _globalUsers.GetOrAdd(id.ToIdString(), _ => userFactory(id));

    internal SocketGlobalUser? RemoveGlobalUser(string id) =>
        _globalUsers.TryRemove(id, out SocketGlobalUser? user) ? user : null;

    #endregion

    #region Guild Users

    internal SocketGuildUser? GetGuildUser(ulong id) => _guildUsers.GetValueOrDefault(id);

    internal SocketGuildUser GetOrAddGuildUser(ulong id, Func<ulong, SocketGuildUser> userFactory) =>
        _guildUsers.GetOrAdd(id, _ => userFactory(id));

    internal SocketGuildUser? RemoveGuildUser(ulong id) =>
        _guildUsers.TryRemove(id, out SocketGuildUser? user) ? user : null;

    #endregion
}
