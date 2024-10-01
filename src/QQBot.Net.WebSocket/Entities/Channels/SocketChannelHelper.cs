using QQBot.API;

namespace QQBot.WebSocket;

internal static class SocketChannelHelper
{
    public static async Task UpdateAsync(SocketGuildChannel channel, RequestOptions? options)
    {
        Channel model = await channel.Client.ApiClient.GetChannelAsync(channel.Id, options).ConfigureAwait(false);
        channel.Update(channel.Client.State, model);
    }

    public static void AddMessage(ISocketMessageChannel channel, QQBotSocketClient client, SocketMessage message)
    {
        switch (channel)
        {
            case SocketUserChannel userChannel:
                userChannel.AddMessage(message);
                break;
            case SocketGroupChannel groupChannel:
                groupChannel.AddMessage(message);
                break;
            case SocketTextChannel textChannel:
                textChannel.AddMessage(message);
                break;
            case SocketDMChannel dmChannel:
                dmChannel.AddMessage(message);
                break;
            default:
                throw new NotSupportedException($"Unexpected {nameof(ISocketMessageChannel)} type.");
        }
    }
}
