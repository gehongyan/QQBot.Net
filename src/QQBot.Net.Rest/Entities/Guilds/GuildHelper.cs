using QQBot.API;

namespace QQBot.Rest;

internal static class GuildHelper
{

    public static async Task<RestGuildMember> GetUserAsync(IGuild guild, BaseQQBotClient client,
        ulong id, RequestOptions? options)
    {
        Member model = await client.ApiClient.GetGuildMemberAsync(guild.Id, id, options).ConfigureAwait(false);
        if (model.User is null)
            throw new InvalidOperationException("User not found in guild.");
        return RestGuildMember.Create(client, guild, model.User, model);
    }
}
