namespace QQBot;

/// <summary>
///     表示键盘按钮的权限类型。
/// </summary>
public enum ButtonPermission
{
    /// <summary>
    ///     指定用户可操作
    /// </summary>
    SpecificUser = 0,

    /// <summary>
    ///     仅管理者可操作
    /// </summary>
    AdministratorOnly = 1,

    /// <summary>
    ///     所有人可操作
    /// </summary>
    Everyone = 2,

    /// <summary>
    ///     指定身份组可操作（仅频道可用）
    /// </summary>
    SpecificRole = 3
}
