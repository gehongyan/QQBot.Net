using QQBot.API;

namespace QQBot.WebSocket;

internal static  class SocketUserHelper
{
    public static async Task UpdateAsync(SocketGuildMember user, QQBotSocketClient client, RequestOptions? options)
    {
        Member model = await client.ApiClient.GetGuildMemberAsync(user.Guild.Id, user.Id, options).ConfigureAwait(false);
        if (model.User is null)
            throw new InvalidOperationException("User not found.");
        user.Update(client.State, model.User, model);
    }
}
