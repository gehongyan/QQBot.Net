namespace QQBot;

/// <summary>
///     表示一个通用的按钮内容构建器。
/// </summary>
public interface IKeyboardBuilder
{
    /// <summary>
    ///     将当前构建器构建为一个按钮内容。
    /// </summary>
    /// <returns> 构建的按钮内容。 </returns>
    IKeyboard Build();
}