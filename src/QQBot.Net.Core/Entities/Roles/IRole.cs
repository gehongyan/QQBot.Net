namespace QQBot;

/// <summary>
///     表示一个子频道身份组。
/// </summary>
public interface IRole : IEntity<uint>
{
    /// <summary>
    ///     获取拥有此角色的频道。
    /// </summary>
    IGuild Guild { get; }

    /// <summary>
    ///     获取此身份组的名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     获取此身份组的类型。
    /// </summary>
    RoleType Type { get; }

    /// <summary>
    ///     获取此身份组的颜色。
    /// </summary>
    AlphaColor Color { get; }

    /// <summary>
    ///     获取拥有此身份组的用户是否在用户列表中与普通在线成员分开显示。
    /// </summary>
    bool IsHoisted { get; }

    /// <summary>
    ///     获取拥有此身分组的用户数量。
    /// </summary>
    int MemberCount { get; }

    /// <summary>
    ///     获取可以拥有此身份组的最大用户数量。
    /// </summary>
    int MaxMembers { get; }
}
