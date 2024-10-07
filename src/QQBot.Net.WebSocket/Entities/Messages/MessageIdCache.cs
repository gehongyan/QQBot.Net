using System.Collections.Concurrent;

namespace QQBot.WebSocket;

internal class MessageIdCache
{
    private readonly ConcurrentHashSet<string> _messageIds;
    private readonly ConcurrentQueue<string> _orderedMessageIds;
    private const int Size = 10;

    public MessageIdCache()
    {
        _messageIds = [];
        _orderedMessageIds = [];
    }

    public bool TryAdd(string id)
    {
        if (!_messageIds.TryAdd(id))
            return false;
        _orderedMessageIds.Enqueue(id);
        while (_orderedMessageIds.Count > Size
               && _orderedMessageIds.TryDequeue(out string? msgId))
            _messageIds.TryRemove(msgId);
        return true;
    }

    public bool Contains(string id) => _messageIds.Contains(id);

    public void Clear()
    {
        _messageIds.Clear();
        _orderedMessageIds.Clear();
    }
}
