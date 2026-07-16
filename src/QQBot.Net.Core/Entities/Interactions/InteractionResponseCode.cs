namespace QQBot;

/// <summary>
///     表示对互动事件的处理结果。
/// </summary>
public enum InteractionResponseCode
{
    /// <summary>
    ///     操作成功。
    /// </summary>
    Success = 0,

    /// <summary>
    ///     操作失败。
    /// </summary>
    Failed = 1,

    /// <summary>
    ///     操作过于频繁。
    /// </summary>
    TooFrequent = 2,

    /// <summary>
    ///     重复操作。
    /// </summary>
    Duplicate = 3,

    /// <summary>
    ///     用户没有操作权限。
    /// </summary>
    PermissionDenied = 4,

    /// <summary>
    ///     仅管理员可以操作。
    /// </summary>
    AdministratorOnly = 5
}
