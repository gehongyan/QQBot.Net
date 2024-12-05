namespace QQBot;

/// <summary>
///     表示一个应用程序接口权限。
/// </summary>
public class ApplicationPermission
{
    /// <summary>
    ///     获取此权限的请求方法。
    /// </summary>
    public HttpMethod Method { get; }

    /// <summary>
    ///     获取此权限的路径。
    /// </summary>
    public string Path { get; }

    /// <summary>
    ///     获取此权限的描述。
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     获取此权限是否已授权。
    /// </summary>
    public bool AuthStatus { get; }

    internal ApplicationPermission(HttpMethod method, string path, string description, bool authStatus)
    {
        Method = method;
        Path = path;
        Description = description;
        AuthStatus = authStatus;
    }
}
