namespace QQBot;

/// <summary>
///     表示机器人在指定 QQ 群内的状态信息。
/// </summary>
public class GroupBotState
{
    /// <summary>
    ///     获取机器人在此群内的成员标识符。
    /// </summary>
    public Guid MemberId { get; }

    /// <summary>
    ///     获取机器人加入此群的时间。
    /// </summary>
    public DateTimeOffset JoinedAt { get; }

    /// <summary>
    ///     获取机器人是否被允许在此群内发送主动消息。
    /// </summary>
    public bool AllowsProactiveMessages { get; }

    /// <summary>
    ///     获取机器人在此群内接收消息的设置。
    /// </summary>
    public GroupMessageReceiveSetting ReceiveSetting { get; }

    /// <summary>
    ///     获取机器人在此群内的角色。
    /// </summary>
    public GroupMemberRole Role { get; }

    internal GroupBotState(Guid memberId, DateTimeOffset joinedAt, bool allowsProactiveMessages,
        GroupMessageReceiveSetting receiveSetting, GroupMemberRole role)
    {
        MemberId = memberId;
        JoinedAt = joinedAt;
        AllowsProactiveMessages = allowsProactiveMessages;
        ReceiveSetting = receiveSetting;
        Role = role;
    }
}
