using QQBot.API;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class GroupHelper
{
    public static async Task<GroupInfo> GetInfoAsync(IGroupChannel channel, BaseQQBotClient client,
        RequestOptions? options)
    {
        GetGroupInfoResponse model = await client.ApiClient
            .GetGroupInfoAsync(channel.Id, options).ConfigureAwait(false);
        return new GroupInfo(model.GroupOpenId, model.GroupName ?? string.Empty,
            model.GroupFingerMemo ?? string.Empty, model.GroupClassText ?? string.Empty,
            model.GroupTags ?? [], model.GroupMemberNum);
    }

    public static async Task<GroupBotState> GetBotStateAsync(IGroupChannel channel, BaseQQBotClient client,
        RequestOptions? options)
    {
        GetGroupBotStateResponse model = await client.ApiClient
            .GetGroupBotStateAsync(channel.Id, options).ConfigureAwait(false);
        return new GroupBotState(model.MemberOpenId, model.JoinedAt, model.AllowProactiveMsg,
            model.RecvMsgSetting, model.MemberRole);
    }

    public static async Task<IGroupMember?> GetMemberAsync(IGroupChannel channel, BaseQQBotClient client,
        Guid id, RequestOptions? options)
    {
        GroupMember model = await client.ApiClient
            .GetGroupMemberAsync(channel.Id, id, options).ConfigureAwait(false);
        return RestGroupMember.Create(client, model);
    }

    public static IAsyncEnumerable<IReadOnlyCollection<IGroupMember>> GetMembersAsync(IGroupChannel channel,
        BaseQQBotClient client, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<IGroupMember>(
            QQBotConfig.MaxGroupMembersPerBatch,
            async (info, _) =>
            {
                GetGroupMembersResponse response = await client.ApiClient
                    .GetGroupMembersAsync(channel.Id, info.Cookie, options).ConfigureAwait(false);
                info.Cookie = response.NextCursor;
                return response.Members.Select(x => (IGroupMember)RestGroupMember.Create(client, x)).ToArray();
            },
            nextPage: (info, _) => !string.IsNullOrEmpty(info.Cookie));
    }
}
