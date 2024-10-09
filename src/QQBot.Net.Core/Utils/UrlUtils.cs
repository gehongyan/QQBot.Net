namespace QQBot;

/// <summary>
///     提供有关网络地址的工具方法。
/// </summary>
public static class UrlUtils
{
    /// <summary>
    ///     获取用户头像地址。
    /// </summary>
    /// <param name="appId"> 应用程序 ID。 </param>
    /// <param name="userOpenId"> 用户的开开放 ID。 </param>
    /// <returns></returns>
    public static string GetUserAvatarUrl(int appId, Guid userOpenId) =>
        $"https://q.qlogo.cn/qqapp/{appId}/{userOpenId.ToString("N").ToUpperInvariant()}/100";
}
