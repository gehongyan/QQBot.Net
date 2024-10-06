namespace QQBot;

/// <summary>
///     表示一个通用的模板。
/// </summary>
public interface IArk
{
    /// <summary>
    ///     获取模板的 ID。
    /// </summary>
    int TemplateId { get;}

    /// <summary>
    ///     获取模板的参数。
    /// </summary>
    IReadOnlyDictionary<string, IArkParameter> Parameters { get; }
}