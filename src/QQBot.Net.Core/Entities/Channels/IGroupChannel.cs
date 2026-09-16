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
}
