namespace QQBot;

/// <summary>
///     表示身份组的类型。
/// </summary>
public enum RoleType
{
    /// <summary>
    ///     身份组是由用户创建的。
    /// </summary>
    UserCreated = 0,

    /// <summary>
    ///     身份组是默认的 <c>@全体成员</c> 全体成员身份组。
    /// </summary>
    Everyone = 1,

    /// <summary>
    ///     身份组是由系统默认创建的管理员身份组。
    /// </summary>
    Manager = 2,

    /// <summary>
    ///     身份组是由系统默认创建的群主或创建者。
    /// </summary>
    Owner = 4,

    /// <summary>
    ///     身份组是由系统默认创建的子频道管理员。
    /// </summary>
    ChannelManager = 5,

    /// <summary>
    ///     身份组是由系统默认创建的访客身份组。
    /// </summary>
    Guest = 6,

    /// <summary>
    ///     身份组是由系统默认创建的分组管理员。
    /// </summary>
    CategoryManager = 7
}
