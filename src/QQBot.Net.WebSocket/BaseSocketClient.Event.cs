namespace QQBot.WebSocket;

public abstract partial class BaseSocketClient
{
    /// <summary>
    ///     当接收到新消息时引发。
    /// </summary>
    /// <remarks>
    ///     事件参数：
    ///     <list type="number">
    ///     <item> <see cref="QQBot.WebSocket.SocketMessage"/> 参数是新接收到的消息。 </item>
    ///     </list>
    /// </remarks>
    public event Func<SocketMessage, Task> MessageReceived
    {
        add => _messageReceivedEvent.Add(value);
        remove => _messageReceivedEvent.Remove(value);
    }

    internal readonly AsyncEvent<Func<SocketMessage, Task>> _messageReceivedEvent = new();
}
