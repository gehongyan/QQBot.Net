using System.Collections.Immutable;

namespace QQBot;

/// <summary>
///     表示一个在执行 API 请求后从 QQBot 接收到的 JSON 数据中解析出的错误。
/// </summary>
public struct QQBotJsonError
{
    /// <summary>
    ///     获取错误的 JSON 路径。
    /// </summary>
    public string Path { get; }

    /// <summary>
    ///     获取与路径上的特定属性关联的错误集合。
    /// </summary>
    public IReadOnlyCollection<QQBotError> Errors { get; }

    internal QQBotJsonError(string path, QQBotError[] errors)
    {
        Path = path;
        Errors = errors.ToImmutableArray();
    }
}

/// <summary>
///     表示一个 QQBot 返回的错误。
/// </summary>
public struct QQBotError
{
    /// <summary>
    ///     获取错误的代码。
    /// </summary>
    public string Code { get; }

    /// <summary>
    ///     获取错误的原因。
    /// </summary>
    public string Message { get; }

    internal QQBotError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
