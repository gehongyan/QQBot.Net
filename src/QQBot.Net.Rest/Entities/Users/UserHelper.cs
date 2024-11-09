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
}
