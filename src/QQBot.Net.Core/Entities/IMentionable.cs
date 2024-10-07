namespace QQBot;

/// <summary>
///     表示一个可以被提及的实体对象。
/// </summary>
public interface IMentionable
{
    /// <summary>
    ///     返回一个提及此对象的格式化字符串。
    /// </summary>
    string Mention { get; }
}
