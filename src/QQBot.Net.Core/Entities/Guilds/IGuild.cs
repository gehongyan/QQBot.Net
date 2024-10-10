namespace QQBot;

/// <summary>
///     表示一个频道.
/// </summary>
public interface IGuild : IEntity<ulong>
{
    /// <summary>
    ///     获取此频道的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此频道创建者用户的 ID。
    /// </summary>
    ulong OwnerId { get; }

    /// <summary>
    ///     获取当前用户是否是此频道的创建者。
    /// </summary>
    bool IsOwner { get; }

    /// <summary>
    ///     获取此频道的成员数量。
    /// </summary>
    int MemberCount { get; }

    /// <summary>
    ///     获取可以加入到此频道的最大成员数量。
    /// </summary>
    int MaxMembers { get; }

    /// <summary>
    ///     获取此频道的描述。
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     获取当前用户加入此频道的时间。
    /// </summary>
    DateTimeOffset JoinedAt { get; }

    /// <summary>
    ///     确定此服务器实体是否已准备就绪以供用户代码访问。
    /// </summary>
    /// <remarks>
    ///     <note>
    ///         此属性仅对基于网关连接的客户端有意义。
    ///     </note>
    ///     此属性为 <c>true</c> 表示，此服务器实体已完整缓存基础数据，并与网关同步。 <br />
    ///     缓存基础数据包括服务器基本信息、频道、角色、频道权限重写、当前用户在服务器内的昵称。
    /// </remarks>
    bool IsAvailable { get; }

    /// <summary>
    ///     获取此服务器内的用户。
    /// </summary>
    /// <remarks>
    ///     此方法获取加入到此服务器内的用户。
    ///     <note>
    ///         此方法在网关的实现中可能返回 <c>null</c>，因为在大型服务器中，用户列表的缓存可能不完整。
    ///     </note>
    /// </remarks>
    /// <param name="id"> 要获取的用户的 ID。 </param>
    /// <param name="mode"> 指示当前方法是否应该仅从缓存中获取结果，还是可以通过 API 请求获取数据。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含与指定的 <paramref name="id"/> 关联的用户；如果未找到，则返回 <c>null</c>。 </returns>
    Task<IGuildMember?> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);
}
