using QQBot.API;
using QQBot.API.Rest;

namespace QQBot.WebSocket;

internal static class SocketGuildHelper
{
    public static async Task UpdateAsync(SocketGuild guild, QQBotSocketClient client, RequestOptions? options)
    {
        Guild guildModel = await client.ApiClient.GetGuildAsync(guild.Id, options).ConfigureAwait(false);
        guild.Update(client.State, guildModel);
        if (client.StartupCacheFetchData.HasFlag(StartupCacheFetchData.Channels))
        {
            IReadOnlyCollection<Channel> channelModels = await client.ApiClient.GetChannelsAsync(guild.Id, options).ConfigureAwait(false);
            guild.Update(client.State, channelModels);
        }
        if (client.StartupCacheFetchData.HasFlag(StartupCacheFetchData.Roles))
        {
            GetGuildRolesResponse rolesModel = await client.ApiClient.GetGuildRolesAsync(guild.Id, options).ConfigureAwait(false);
            guild.Update(client.State, rolesModel);
        }

        guild.IsAvailable = true;
    }
}
