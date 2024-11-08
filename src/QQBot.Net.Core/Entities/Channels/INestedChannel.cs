namespace QQBot;

/// <summary>
///     表示一个通用的嵌套子频道，即可以嵌套在分组子频道中的频道子频道。
/// </summary>
public interface INestedChannel : IGuildChannel
{
    /// <summary>
    ///     获取此嵌套子频道在子频道列表中所属的分组子频道的 ID。
    /// </summary>
    /// <remarks> 如果当前子频道不属于任何分组子频道，则会返回 <c>null</c>。 </remarks>
    ulong? CategoryId { get; }

    /// <summary>
    ///     获取此子频道的私密类型。
    /// </summary>
    PrivateType? PrivateType { get; }

    /// <summary>
    ///     获取此子频道的发言权限。
    /// </summary>
    SpeakPermission? SpeakPermission { get; }

    /// <summary>
    ///     获取当前用户在此子频道的权限。
    /// </summary>
    ChannelPermission? Permission { get; }
}
