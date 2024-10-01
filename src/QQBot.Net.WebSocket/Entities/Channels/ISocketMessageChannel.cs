namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的消息频道，可以发送和接收消息。
/// </summary>
public interface ISocketMessageChannel : IMessageChannel
{
    /// <summary>
    ///     获取此频道缓存的所有消息。
    /// </summary>
    /// <remarks>
    ///     <note type="warning">
    ///         要想通过此属性获取缓存的消息，需要启用缓存功能，否则此属性将始终返回空集合。缓存功能是默认禁用的，要想启用缓存，请参考
    ///         <see cref="QQBot.WebSocket.QQBotSocketConfig.MessageCacheSize"/>。
    ///     </note>
    ///     <br />
    ///     此属性从本地的内存缓存中获取消息实体，不会向 KOOK 发送额外的 API 请求。所获取的消息也可能是已经被删除的消息。
    /// </remarks>
    IReadOnlyCollection<SocketMessage> CachedMessages { get; }
}
