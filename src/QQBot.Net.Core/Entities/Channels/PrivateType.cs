namespace QQBot;

/// <summary>
///     表示一个子频道的私密类型。
/// </summary>
public enum PrivateType
{
    /// <summary>
    ///     公开子频道。
    /// </summary>
    Public = 0,

    /// <summary>
    ///     群主及管理员可见 。
    /// </summary>
    Administrators = 1,

    /// <summary>
    ///     群主、管理员、及指定成员可见。
    /// </summary>
    SpecifiedMembers = 2
}
