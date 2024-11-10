namespace QQBot.Net.Queue;

/// <summary>
///     表示一个通用的消息队列抽象类。
/// </summary>
public abstract class BaseMessageQueue : IMessageQueue
{
    /// <summary>
    ///     初始化一个 <see cref="BaseMessageQueue"/> 类的新实例。
    /// </summary>
    /// <param name="eventHandler"> 用于处理消息的事件处理程序。 </param>
    protected BaseMessageQueue(Func<int, string, object, Task> eventHandler)
    {
        EventHandler = eventHandler;
    }

    /// <summary>
    ///     获取消息队列的事件处理程序。
    /// </summary>
    protected Func<int, string, object, Task> EventHandler { get; }

    /// <summary>
    ///     启动消息队列的处理。
    /// </summary>
    /// <param name="cancellationToken"> 用于取消该操作的取消令牌。 </param>
    /// <returns> 表示一个异步启动操作的任务。 </returns>
    public abstract Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     停止消息队列的处理。
    /// </summary>
    /// <param name="cancellationToken"> 用于取消该操作的取消令牌。 </param>
    /// <returns> 表示一个异步停止操作的任务。 </returns>
    public abstract Task StopAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task EnqueueAsync(int sequence,
        string type, object payload,
        CancellationToken cancellationToken = default);
}
