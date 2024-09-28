using QQBot.API;

namespace QQBot.WebSocket;

internal static class SocketChannelHelper
{
    public static async Task UpdateAsync(SocketGuildChannel channel, RequestOptions? options)
    {
        Channel model = await channel.Client.ApiClient.GetChannelAsync(channel.Id, options).ConfigureAwait(false);
        channel.Update(channel.Client.State, model);
    }
}
