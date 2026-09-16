namespace QQBot;

/// <summary>
///     表示一个群组子频道，即 QQ 群。
/// </summary>
public interface IGroupChannel : IMessageChannel, IMediaUploadChannel, IEntity<Guid>
{
    /// <summary>
    ///     获取此群组子频道的唯一标识符。
    /// </summary>
    new Guid Id { get; }

    /// <summary>
    ///     撤回此群组子频道内的消息。
    /// </summary>
    /// <param name="messageId"> 要撤回的消息 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤回操作的任务。 </returns>
    Task DeleteMessageAsync(string messageId, RequestOptions? options = null);

    /// <summary>
    ///     撤回此群组子频道内的消息。
    /// </summary>
    /// <param name="message"> 要撤回的消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步撤回操作的任务。 </returns>
    Task DeleteMessageAsync(IUserMessage message, RequestOptions? options = null);

    /// <summary>
    ///     获取此群的基本信息。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此群的基本信息。 </returns>
    Task<GroupInfo> GetInfoAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取当前机器人在此群内的状态。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含当前机器人在此群内的状态。 </returns>
    Task<GroupBotState> GetBotStateAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取此群内的所有成员。
    /// </summary>
    /// <remarks>
    ///     此方法以分页的形式获取此群内的所有成员。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，包含此群内的所有成员。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<IGroupMember>> GetMembersAsync(RequestOptions? options = null);

    /// <summary>
    ///     获取此群内指定的成员。
    /// </summary>
    /// <param name="id"> 要获取的成员的 ID。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的成员；若未找到，则为 <c>null</c>。 </returns>
    Task<IGroupMember?> GetMemberAsync(Guid id, RequestOptions? options = null);

    /// <summary>
    ///     获取此群的入群申请，以分页形式返回。
    /// </summary>
    /// <remarks>
    ///     此方法以分页的形式获取此群的入群申请。机器人需拥有群管理员身份。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，包含此群的入群申请。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<GroupJoinRequest>> GetJoinRequestsAsync(RequestOptions? options = null);

    /// <summary>
    ///     通过一条入群申请。
    /// </summary>
    /// <param name="request"> 要通过的入群申请。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步审批操作的任务。 </returns>
    Task ApproveJoinRequestAsync(GroupJoinRequest request, RequestOptions? options = null);

    /// <summary>
    ///     通过一条入群申请。
    /// </summary>
    /// <param name="memberId"> 申请人的用户标识符。 </param>
    /// <param name="joinRequestId"> 申请的唯一标识符；若已知则一并回传。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步审批操作的任务。 </returns>
    Task ApproveJoinRequestAsync(Guid memberId, string? joinRequestId = null, RequestOptions? options = null);

    /// <summary>
    ///     拒绝一条入群申请。
    /// </summary>
    /// <param name="request"> 要拒绝的入群申请。 </param>
    /// <param name="reason"> 拒绝理由。 </param>
    /// <param name="addToBlacklist"> 是否在拒绝的同时将申请人加入群黑名单。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步审批操作的任务。 </returns>
    Task DeclineJoinRequestAsync(GroupJoinRequest request, string? reason = null,
        bool addToBlacklist = false, RequestOptions? options = null);

    /// <summary>
    ///     拒绝一条入群申请。
    /// </summary>
    /// <param name="memberId"> 申请人的用户标识符。 </param>
    /// <param name="joinRequestId"> 申请的唯一标识符；若已知则一并回传。 </param>
    /// <param name="reason"> 拒绝理由。 </param>
    /// <param name="addToBlacklist"> 是否在拒绝的同时将申请人加入群黑名单。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步审批操作的任务。 </returns>
    Task DeclineJoinRequestAsync(Guid memberId, string? joinRequestId = null,
        string? reason = null, bool addToBlacklist = false, RequestOptions? options = null);

    /// <summary>
    ///     获取此群的禁言状态。
    /// </summary>
    /// <remarks>
    ///     返回全员禁言规则（包含定时与周期规则）以及当前处于禁言状态的成员列表。机器人需拥有群管理员身份。
    /// </remarks>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此群的禁言状态。 </returns>
    Task<GroupMuteSetting> GetMuteSettingAsync(RequestOptions? options = null);

    /// <summary>
    ///     禁言此群内的指定成员。
    /// </summary>
    /// <remarks>
    ///     机器人需拥有群管理员身份，最大禁言时长为 30 天，且仅可禁言普通成员。
    /// </remarks>
    /// <param name="memberId"> 要禁言的成员的标识符。 </param>
    /// <param name="expiresAt"> 禁言到期时间。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步禁言操作的任务。 </returns>
    Task MuteMemberAsync(Guid memberId, DateTimeOffset expiresAt, RequestOptions? options = null);

    /// <summary>
    ///     禁言此群内的指定成员。
    /// </summary>
    /// <remarks>
    ///     机器人需拥有群管理员身份，最大禁言时长为 30 天，且仅可禁言普通成员。
    /// </remarks>
    /// <param name="memberId"> 要禁言的成员的标识符。 </param>
    /// <param name="duration"> 自当前时间起的禁言时长。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步禁言操作的任务。 </returns>
    Task MuteMemberAsync(Guid memberId, TimeSpan duration, RequestOptions? options = null);

    /// <summary>
    ///     解除此群内指定成员的禁言。
    /// </summary>
    /// <param name="memberId"> 要解除禁言的成员的标识符。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步解除禁言操作的任务。 </returns>
    Task UnmuteMemberAsync(Guid memberId, RequestOptions? options = null);

    /// <summary>
    ///     批量移除此群内的成员。
    /// </summary>
    /// <remarks>
    ///     单次最多移除 20 个成员。机器人需拥有群管理员身份。
    /// </remarks>
    /// <param name="memberIds"> 要移除的成员的标识符集合。 </param>
    /// <param name="addToBlacklist"> 是否在移除的同时将成员加入群黑名单。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步移除操作的任务。任务的结果包含移除操作的结果。 </returns>
    Task<GroupRemoveMembersResult> RemoveMembersAsync(IEnumerable<Guid> memberIds,
        bool addToBlacklist = false, RequestOptions? options = null);

    /// <summary>
    ///     批量移除此群内的成员。
    /// </summary>
    /// <remarks>
    ///     单次最多移除 20 个成员。机器人需拥有群管理员身份。
    /// </remarks>
    /// <param name="members"> 要移除的成员集合。 </param>
    /// <param name="addToBlacklist"> 是否在移除的同时将成员加入群黑名单。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步移除操作的任务。任务的结果包含移除操作的结果。 </returns>
    Task<GroupRemoveMembersResult> RemoveMembersAsync(IEnumerable<IGroupMember> members,
        bool addToBlacklist = false, RequestOptions? options = null);

    /// <summary>
    ///     获取此群的黑名单，以分页形式返回。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，包含此群黑名单中的用户。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<GroupBlacklistUser>> GetBlacklistAsync(RequestOptions? options = null);

    /// <summary>
    ///     将指定成员加入此群的黑名单。
    /// </summary>
    /// <remarks>
    ///     单次最多操作 20 个成员，且仅当目标成员不在群中时才能加入黑名单。
    /// </remarks>
    /// <param name="memberIds"> 要加入黑名单的成员的标识符集合。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。任务的结果包含操作失败的成员标识符集合；若为空表示全部成功。 </returns>
    Task<IReadOnlyCollection<Guid>> AddToBlacklistAsync(IEnumerable<Guid> memberIds, RequestOptions? options = null);

    /// <summary>
    ///     将指定成员加入此群的黑名单。
    /// </summary>
    /// <remarks>
    ///     单次最多操作 20 个成员，且仅当目标成员不在群中时才能加入黑名单。
    /// </remarks>
    /// <param name="members"> 要加入黑名单的成员集合。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。任务的结果包含操作失败的成员标识符集合；若为空表示全部成功。 </returns>
    Task<IReadOnlyCollection<Guid>> AddToBlacklistAsync(IEnumerable<IGroupMember> members, RequestOptions? options = null);

    /// <summary>
    ///     将指定成员移出此群的黑名单。
    /// </summary>
    /// <remarks>
    ///     单次最多操作 20 个成员。
    /// </remarks>
    /// <param name="memberIds"> 要移出黑名单的成员的标识符集合。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。任务的结果包含操作失败的成员标识符集合；若为空表示全部成功。 </returns>
    Task<IReadOnlyCollection<Guid>> RemoveFromBlacklistAsync(IEnumerable<Guid> memberIds, RequestOptions? options = null);

    /// <summary>
    ///     将指定成员移出此群的黑名单。
    /// </summary>
    /// <remarks>
    ///     单次最多操作 20 个成员。
    /// </remarks>
    /// <param name="members"> 要移出黑名单的成员集合。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。任务的结果包含操作失败的成员标识符集合；若为空表示全部成功。 </returns>
    Task<IReadOnlyCollection<Guid>> RemoveFromBlacklistAsync(IEnumerable<IGroupMember> members, RequestOptions? options = null);
}
