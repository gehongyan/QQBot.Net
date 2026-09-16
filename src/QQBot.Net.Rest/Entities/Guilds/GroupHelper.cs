using QQBot.API;
using QQBot.API.Rest;
using ApiGroupJoinRequest = QQBot.API.GroupJoinRequest;

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

    public static IAsyncEnumerable<IReadOnlyCollection<GroupJoinRequest>> GetJoinRequestsAsync(IGroupChannel channel,
        BaseQQBotClient client, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<GroupJoinRequest>(
            QQBotConfig.MaxGroupJoinRequestsPerBatch,
            async (info, _) =>
            {
                GetGroupJoinRequestListResponse response = await client.ApiClient
                    .GetGroupJoinRequestListAsync(channel.Id, info.Cookie,
                        QQBotConfig.MaxGroupJoinRequestsPerBatch, options).ConfigureAwait(false);
                info.Cookie = response.NextCursor;
                return response.List.Select(ToGroupJoinRequest).ToArray();
            },
            nextPage: (info, _) => !string.IsNullOrEmpty(info.Cookie));
    }

    public static Task ApproveJoinRequestAsync(IGroupChannel channel, BaseQQBotClient client,
        Guid memberId, string? joinRequestId, RequestOptions? options)
    {
        ApproveGroupJoinRequestParams args = new()
        {
            Op = "approve",
            JoinRequestId = joinRequestId
        };
        return client.ApiClient.ApproveGroupJoinRequestAsync(channel.Id, memberId, args, options);
    }

    public static Task DeclineJoinRequestAsync(IGroupChannel channel, BaseQQBotClient client,
        Guid memberId, string? joinRequestId, string? reason, bool addToBlacklist, RequestOptions? options)
    {
        ApproveGroupJoinRequestParams args = new()
        {
            Op = "decline",
            JoinRequestId = joinRequestId,
            RejectReason = reason,
            AddToMemberBlacklist = addToBlacklist ? true : null
        };
        return client.ApiClient.ApproveGroupJoinRequestAsync(channel.Id, memberId, args, options);
    }

    private static GroupJoinRequest ToGroupJoinRequest(ApiGroupJoinRequest model)
    {
        GroupJoinVerifyInfo? verifyInfo = null;
        if (model.VerifyInfo is { } info)
        {
            IReadOnlyCollection<GroupJoinReviewQuestion> questions = info.ReviewQaList is { } list
                ? list.Select(x => new GroupJoinReviewQuestion(x.Question ?? string.Empty, x.Answer ?? string.Empty)).ToArray()
                : [];
            verifyInfo = new GroupJoinVerifyInfo(info.Method, info.VerifyMessage, questions);
        }

        return new GroupJoinRequest(model.JoinRequestId, model.MemberOpenId, model.Username ?? string.Empty,
            model.UnionOpenId, model.Bot, model.ApplyAt, model.ApplySource,
            model.InvitedBy is { } invitedBy && invitedBy != Guid.Empty ? invitedBy : null,
            model.RiskTips, verifyInfo);
    }
}
