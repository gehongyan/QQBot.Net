using System.Collections.Immutable;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class RoleHelper
{
    #region Users

    public static async IAsyncEnumerable<IReadOnlyCollection<RestGuildMember>> GetUsersAsync(
        IRole role, BaseQQBotClient client, int? limit, RequestOptions? options)
    {
        string startIndex = "0";
        int lastPageSize = 0;
        while (lastPageSize < QQBotConfig.MaxMembersPerBatch)
        {
            GetGuildRoleMembersParams args = new()
            {
                Limit = QQBotConfig.MaxMembersPerBatch,
                StartIndex = startIndex
            };
            GetGuildRoleMembersResponse model = await client.ApiClient
                .GetGuildRoleMembersAsync(role.Guild.Id, role.Id, args, options).ConfigureAwait(false);
            yield return model.Members
                .Select(x => RestGuildMember.Create(
                    client, role.Guild, x.User ?? throw new InvalidOperationException("User not found in guild."), x))
                .ToImmutableArray();
            startIndex = model.Next;
            lastPageSize = model.Members.Length;
        }
    }

    #endregion
}
