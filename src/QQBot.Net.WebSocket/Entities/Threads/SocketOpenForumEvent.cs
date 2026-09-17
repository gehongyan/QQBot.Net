using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的开放论坛事件。
/// </summary>
/// <remarks>
///     开放论坛事件为公域话题子频道内的主题、评论或回复的创建、更新与删除通知。此类事件仅携带频道与操作者信息，不包含内容详情。
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketOpenForumEvent
{
    /// <summary>
    ///     获取此事件所属的频道。
    /// </summary>
    public SocketGuild Guild { get; }

    /// <summary>
    ///     获取此事件所属的论坛子频道。
    /// </summary>
    public SocketForumChannel Channel { get; }

    /// <summary>
    ///     获取触发此事件的操作者用户的 ID。
    /// </summary>
    public ulong AuthorId { get; }

    private SocketOpenForumEvent(SocketForumChannel channel, ulong authorId)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuthorId = authorId;
    }

    internal static SocketOpenForumEvent Create(SocketForumChannel channel, API.Gateway.OpenForumEvent model) =>
        new(channel, model.AuthorId);

    private string DebuggerDisplay => $"{Channel.Name} ({AuthorId})";
}
