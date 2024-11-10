using System.Collections.Concurrent;

namespace QQBot.WebSocket;

internal class MessageCache
{
    private readonly ConcurrentDictionary<string, SocketMessage> _messages;
    private readonly ConcurrentQueue<(string Id, DateTimeOffset Timestamp)> _orderedMessages;
    private readonly int _size;

    public IReadOnlyCollection<SocketMessage> Messages => _messages.ToReadOnlyCollection();

    public MessageCache(QQBotSocketClient client)
    {
        _size = client.MessageCacheSize;
        _messages = new ConcurrentDictionary<string, SocketMessage>(ConcurrentHashSet.DefaultConcurrencyLevel, (int)(_size * 1.05));
        _orderedMessages = [];
    }

    public void Add(SocketMessage message)
    {
        if (!_messages.TryAdd(message.Id, message))
            return;
        _orderedMessages.Enqueue((message.Id, message.Timestamp));
        while (_orderedMessages.Count > _size
               && _orderedMessages.TryDequeue(out (string Id, DateTimeOffset Timestamp) msg))
            _messages.TryRemove(msg.Id, out _);
    }

    public SocketMessage? Remove(string id) => _messages.TryRemove(id, out SocketMessage? msg) ? msg : null;

    public SocketMessage? Get(string id) => _messages.GetValueOrDefault(id);
}
