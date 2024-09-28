namespace QQBot;

/// <summary>
///     表示子频道发言权限。
/// </summary>
public enum SpeakPermission
{
    /// <summary>
    ///     任何人都可以发言。
    /// </summary>
    Everyone = 1,

    /// <summary>
    ///     群主、管理员、及指定成员可以发言。
    /// </summary>
    SpecifiedMembers = 2,
}
