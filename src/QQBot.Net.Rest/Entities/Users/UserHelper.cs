using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class UserHelper
{
    public static async Task KickAsync(IGuildMember member, BaseQQBotClient client,
        bool addBlacklist, int pruneDays, RequestOptions? options)
    {
        DeleteGuildMemberParams args = new()
        {
            AddBlacklist = addBlacklist,
            DeleteHistoryMessageDays = pruneDays
        };
        await client.ApiClient.DeleteGuildMemberAsync(member.GuildId, member.Id, args, options).ConfigureAwait(false);
    }

    public static async Task AddRolesAsync(IGuildMember user, BaseQQBotClient client,
        IEnumerable<uint> roleIds, RequestOptions? options)
    {
        foreach (uint roleId in roleIds)
            await client.ApiClient.GrantGuildRoleAsync(user.GuildId, user.Id, roleId, options).ConfigureAwait(false);
    }

    public static async Task RemoveRolesAsync(IGuildMember user, BaseQQBotClient client,
        IEnumerable<uint> roleIds, RequestOptions? options)
    {
        foreach (uint roleId in roleIds)
            await client.ApiClient.RevokeGuildRoleAsync(user.GuildId, user.Id, roleId, options).ConfigureAwait(false);
    }
}
