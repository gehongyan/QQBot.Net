namespace QQBot;

/// <summary>
///     表示一个通用的用户。
/// </summary>
public interface IUser : IEntity<string>, IMentionable
{
    /// <summary>
    ///     获取此用户的头像图像的 URL。
    /// </summary>
    string? Avatar { get; }
}
