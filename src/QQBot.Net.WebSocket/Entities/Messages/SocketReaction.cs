using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的表情表态动作。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketReaction
{
    /// <summary>
    ///     获取此表态所属的频道。
    /// </summary>
    public SocketGuild Guild { get; }

    /// <summary>
    ///     获取此表态所在的子频道。
    /// </summary>
    public SocketTextChannel Channel { get; }

    /// <summary>
    ///     获取执行此表态操作的用户的 ID。
    /// </summary>
    public ulong UserId { get; }

    /// <summary>
    ///     获取此表态所针对的目标对象的类型。
    /// </summary>
    public ReactionTargetType TargetType { get; }

    /// <summary>
    ///     获取此表态所针对的目标对象的 ID。
    /// </summary>
    /// <remarks>
    ///     当 <see cref="TargetType"/> 为 <see cref="QQBot.ReactionTargetType.Message"/> 时为消息 ID；
    ///     为其余类型时分别为论坛主题、评论或回复的 ID。
    /// </remarks>
    public string TargetId { get; }

    /// <summary>
    ///     获取此表态所使用的表情符号。
    /// </summary>
    public IEmote Emote { get; }

    private SocketReaction(SocketTextChannel channel, ulong userId,
        ReactionTargetType targetType, string targetId, IEmote emote)
    {
        Guild = channel.Guild;
        Channel = channel;
        UserId = userId;
        TargetType = targetType;
        TargetId = targetId;
        Emote = emote;
    }

    internal static SocketReaction Create(SocketTextChannel channel, API.Gateway.MessageReactionEvent model)
    {
        ReactionTargetType targetType = model.Target.Type switch
        {
            0 => ReactionTargetType.Message,
            1 => ReactionTargetType.Thread,
            2 => ReactionTargetType.Post,
            3 => ReactionTargetType.Reply,
            _ => throw new ArgumentOutOfRangeException(nameof(model), model.Target.Type,
                "Unknown reaction target type.")
        };
        Emote emote = new(model.Emoji.Type, model.Emoji.Id, string.Empty);
        return new SocketReaction(channel, model.UserId, targetType, model.Target.Id, emote);
    }

    private string DebuggerDisplay => $"{Emote} on {TargetType} {TargetId} by {UserId}";
}
