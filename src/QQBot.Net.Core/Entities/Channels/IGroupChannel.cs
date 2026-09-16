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
}
