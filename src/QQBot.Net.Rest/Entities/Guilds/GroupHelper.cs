using QQBot.API;
using QQBot.API.Rest;
using ApiGroupJoinRequest = QQBot.API.GroupJoinRequest;
using ApiGroupGlobalMuteRule = QQBot.API.GroupGlobalMuteRule;
using ApiGroupMuteScheduleRule = QQBot.API.GroupMuteScheduleRule;
using ApiGroupMuteRecurringRule = QQBot.API.GroupMuteRecurringRule;
using ApiGroupMemberMuteState = QQBot.API.GroupMemberMuteState;

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
        return RestGroupMember.Create(client, model, channel.Id);
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
                return response.Members.Select(x => (IGroupMember)RestGroupMember.Create(client, x, channel.Id)).ToArray();
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

    public static async Task<GroupMuteSetting> GetMuteSettingAsync(IGroupChannel channel, BaseQQBotClient client,
        RequestOptions? options)
    {
        GetGroupMuteSettingResponse model = await client.ApiClient
            .GetGroupMuteSettingAsync(channel.Id, options).ConfigureAwait(false);
        GroupGlobalMuteRule globalRule = ToGlobalMuteRule(model.GlobalRule);
        IReadOnlyCollection<GroupMemberMuteState> members = model.Members is { } list
            ? list.Select(ToMemberMuteState).ToArray()
            : [];
        return new GroupMuteSetting(globalRule, members);
    }

    public static Task MuteMemberAsync(Guid groupId, BaseQQBotClient client,
        Guid memberId, DateTimeOffset expiresAt, RequestOptions? options)
    {
        SetGroupMuteSettingParams args = new()
        {
            Members =
            [
                new SetGroupMemberMuteState
                {
                    Op = "add",
                    MemberOpenId = memberId.ToIdString(),
                    MuteExpireAt = expiresAt.ToString("yyyy-MM-ddTHH:mm:ssK")
                }
            ]
        };
        return client.ApiClient.SetGroupMuteSettingAsync(groupId, args, options);
    }

    public static Task UnmuteMemberAsync(Guid groupId, BaseQQBotClient client,
        Guid memberId, RequestOptions? options)
    {
        SetGroupMuteSettingParams args = new()
        {
            Members =
            [
                new SetGroupMemberMuteState
                {
                    Op = "del",
                    MemberOpenId = memberId.ToIdString(),
                    MuteExpireAt = string.Empty
                }
            ]
        };
        return client.ApiClient.SetGroupMuteSettingAsync(groupId, args, options);
    }

    private static GroupGlobalMuteRule ToGlobalMuteRule(ApiGroupGlobalMuteRule? model)
    {
        if (model is null)
            return new GroupGlobalMuteRule(GroupGlobalMuteMode.None, [], []);

        IReadOnlyCollection<GroupMuteScheduleRule> schedule = model.ScheduleRules is { } sr
            ? sr.Select(ToScheduleRule).ToArray()
            : [];
        IReadOnlyCollection<GroupMuteRecurringRule> recurring = model.RecurringRules is { } rr
            ? rr.Select(ToRecurringRule).ToArray()
            : [];
        return new GroupGlobalMuteRule(model.Mode, schedule, recurring);
    }

    private static GroupMuteScheduleRule ToScheduleRule(ApiGroupMuteScheduleRule model) =>
        new(model.TaskId ?? string.Empty, model.StartAt, model.EndAt, model.Enabled);

    private static GroupMuteRecurringRule ToRecurringRule(ApiGroupMuteRecurringRule model)
    {
        IReadOnlyCollection<DayOfWeek> weekdays = model.Weekdays is { } days
            ? days.Select(ToDayOfWeek).ToArray()
            : [];
        TimeOnly start = TimeOnly.TryParse(model.StartTime, out TimeOnly s) ? s : default;
        TimeOnly end = TimeOnly.TryParse(model.EndTime, out TimeOnly e) ? e : default;
        return new GroupMuteRecurringRule(model.TaskId ?? string.Empty, weekdays, start, end, model.Enabled);
    }

    // QQ 使用 1~7 表示周一至周日；.NET DayOfWeek 中周日为 0、周一至周六为 1~6。
    private static DayOfWeek ToDayOfWeek(int weekday) =>
        weekday == 7 ? DayOfWeek.Sunday : (DayOfWeek)weekday;

    private static GroupMemberMuteState ToMemberMuteState(ApiGroupMemberMuteState model) =>
        new(model.MemberOpenId, model.MuteExpireAt, model.Username ?? string.Empty, model.UnionOpenId);

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
