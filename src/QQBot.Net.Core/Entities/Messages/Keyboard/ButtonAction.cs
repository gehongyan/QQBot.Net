namespace QQBot;

/// <summary>
///     表示键盘按钮的动作类型。
/// </summary>
public enum ButtonAction
{
    /// <summary>
    ///     跳转按钮
    /// </summary>
    Jump = 0,

    /// <summary>
    ///     回调按钮
    /// </summary>
    Callback = 1,

    /// <summary>
    ///     指令按钮
    /// </summary>
    Command = 2
}
