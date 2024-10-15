using QQBot.API;
using QQBot.API.Rest;

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

    public static IAsyncEnumerable<IReadOnlyCollection<Member>> GetMembersAsync(
        IGuild guild, BaseQQBotClient client, int? limit, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<Member>(
            QQBotConfig.MaxMembersPerBatch,
            async (info, ct) =>
            {
                GetGuildMembersParams args = new()
                {
                    Limit = info.PageSize
                };
                if (info.Position != null)
                    args.AfterId = info.Position.Value;
                return [..await client.ApiClient.GetGuildMembersAsync(guild.Id, args, options).ConfigureAwait(false)];
            },
            nextPage: (info, lastPage) =>
            {
                if (lastPage.LastOrDefault()?.User?.Id is not { } lastId)
                    return false;
                info.Position = lastId;
                return true;
            },
            start: null,
            count: limit
        );
    }
}
