namespace QQBot;

/// <summary>
///     表示在执行键盘按钮操作前显示的二次确认提示。
/// </summary>
public class KeyboardModal
{
    /// <summary>
    ///     获取二次确认提示的内容。
    /// </summary>
    public string Content { get; }

    /// <summary>
    ///     获取确认按钮上的文本。
    /// </summary>
    /// <remarks>
    ///     为 <see langword="null"/> 时使用 QQ 客户端的默认文本。
    /// </remarks>
    public string? ConfirmText { get; }

    /// <summary>
    ///     获取取消按钮上的文本。
    /// </summary>
    /// <remarks>
    ///     为 <see langword="null"/> 时使用 QQ 客户端的默认文本。
    /// </remarks>
    public string? CancelText { get; }

    internal KeyboardModal(string content, string? confirmText, string? cancelText)
    {
        Content = content;
        ConfirmText = confirmText;
        CancelText = cancelText;
    }
}
