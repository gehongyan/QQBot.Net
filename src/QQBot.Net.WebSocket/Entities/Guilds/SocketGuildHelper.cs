using QQBot.API;

namespace QQBot.WebSocket;

internal static class SocketGuildHelper
{
    public static async Task UpdateAsync(SocketGuild guild, QQBotSocketClient client, RequestOptions? options)
    {
        Guild guildModel = await client.ApiClient.GetGuildAsync(guild.Id, options).ConfigureAwait(false);
        guild.Update(client.State, guildModel);
        IReadOnlyCollection<Channel> channelModels = await client.ApiClient.GetChannelsAsync(guild.Id, options).ConfigureAwait(false);
        guild.Update(client.State, channelModels);
    }
}
