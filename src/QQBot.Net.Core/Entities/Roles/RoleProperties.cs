namespace QQBot;

/// <summary>
///     提供用于创建与修改 <see cref="QQBot.IRole"/> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateRoleAsync(System.Action{QQBot.RoleProperties},QQBot.RequestOptions)" />
public class RoleProperties
{
    /// <summary>
    ///     获取或设置要设置到此角色的名称。
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色的颜色。
    /// </summary>
    public AlphaColor? Color { get; set; }

    /// <summary>
    ///     获取或设置要设置到此角色拥有此角色的用户是否在用户列表中与普通在线成员分开显示。
    /// </summary>
    public bool? IsHoisted { get; set; }
}
