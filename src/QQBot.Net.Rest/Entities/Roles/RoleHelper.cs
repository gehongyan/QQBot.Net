using System.Collections.Immutable;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class RoleHelper
{
    #region Roles

    public static async Task<API.Role> ModifyAsync(IRole role, BaseQQBotClient client,
        Action<RoleProperties>? func, RequestOptions? options)
    {
        RoleProperties properties = new()
        {
            Name = role.Name,
            Color = role.Color,
            IsHoisted = role.IsHoisted
        };
        func?.Invoke(properties);
        ModifyGuildRoleParams args = new()
        {
            Name = properties.Name,
            Color = properties.Color,
            Hoist = properties.IsHoisted
        };
        ModifyGuildRoleResponse model = await client.ApiClient
            .ModifyGuildRoleAsync(role.Guild.Id, role.Id, args, options);
        return model.Role;
    }

    #endregion

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
                Limit = Math.Clamp(limit ?? QQBotConfig.MaxMembersPerBatch, 1, QQBotConfig.MaxMembersPerBatch),
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

    public static async Task DeleteAsync(IRole role, BaseQQBotClient client, RequestOptions? options) =>
        await client.ApiClient.DeleteGuildRoleAsync(role.Guild.Id, role.Id, options).ConfigureAwait(false);
}
