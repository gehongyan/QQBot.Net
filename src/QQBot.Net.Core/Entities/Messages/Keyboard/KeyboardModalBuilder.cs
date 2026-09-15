namespace QQBot;

/// <summary>
///     表示一个用于创建 <see cref="KeyboardModal"/> 实例的构建器。
/// </summary>
public class KeyboardModalBuilder
{
    /// <summary>
    ///     获取二次确认提示内容的最大长度。
    /// </summary>
    public const int MaxContentLength = 100;

    /// <summary>
    ///     获取确认和取消按钮文本的最大长度。
    /// </summary>
    public const int MaxButtonTextLength = 4;

    /// <summary>
    ///     获取或设置二次确认提示的内容。
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    ///     获取或设置确认按钮上的文本。
    /// </summary>
    /// <remarks>
    ///     未设置时使用 QQ 客户端的默认文本。
    /// </remarks>
    public string? ConfirmText { get; set; }

    /// <summary>
    ///     获取或设置取消按钮上的文本。
    /// </summary>
    /// <remarks>
    ///     未设置时使用 QQ 客户端的默认文本。
    /// </remarks>
    public string? CancelText { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="KeyboardModalBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="content"> 二次确认提示的内容。 </param>
    /// <param name="confirmText"> 确认按钮上的文本。 </param>
    /// <param name="cancelText"> 取消按钮上的文本。 </param>
    public KeyboardModalBuilder(string? content = null, string? confirmText = null, string? cancelText = null)
    {
        Content = content;
        ConfirmText = confirmText;
        CancelText = cancelText;
    }

    /// <summary>
    ///     将当前构建器构建为一个 <see cref="KeyboardModal"/> 实例。
    /// </summary>
    /// <returns> 二次确认提示。 </returns>
    /// <exception cref="ArgumentNullException"> <see cref="Content"/> 未设置时引发。 </exception>
    /// <exception cref="ArgumentOutOfRangeException"> 任一文本超过平台允许的最大长度时引发。 </exception>
    /// <exception cref="ArgumentException"> 二次确认提示、确认或取消按钮文本为空字符串时引发。 </exception>
    public KeyboardModal Build()
    {
        if (Content is null)
            throw new ArgumentNullException(nameof(Content));
        if (Content.Length == 0)
            throw new ArgumentException("The content cannot be empty.", nameof(Content));
        if (Content.Length > MaxContentLength)
            throw new ArgumentOutOfRangeException(nameof(Content), $"The content cannot exceed {MaxContentLength} characters.");

        ValidateButtonText(ConfirmText, nameof(ConfirmText));
        ValidateButtonText(CancelText, nameof(CancelText));
        return new KeyboardModal(Content, ConfirmText, CancelText);
    }

    private static void ValidateButtonText(string? text, string parameterName)
    {
        if (text is null)
            return;
        if (text.Length == 0)
            throw new ArgumentException("The button text cannot be empty.", parameterName);
        if (text.Length > MaxButtonTextLength)
            throw new ArgumentOutOfRangeException(parameterName,
                $"The button text cannot exceed {MaxButtonTextLength} characters.");
    }
}
