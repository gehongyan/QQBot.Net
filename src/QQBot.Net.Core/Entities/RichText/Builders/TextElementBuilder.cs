namespace QQBot;

/// <summary>
///     表示一个文本元素的构建器。
/// </summary>
public class TextElementBuilder : IElementBuilder
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Text;

    /// <summary>
    ///     获取或设置此文本元素的文本。
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    ///     获取或设置此文本元素的样式。
    /// </summary>
    public TextStyle Style { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="TextElementBuilder"/> 类的新实例。
    /// </summary>
    public TextElementBuilder()
    {
        Style = TextStyle.None;
    }

    /// <summary>
    ///     初始化一个 <see cref="TextElementBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> 文本。 </param>
    /// <param name="style"> 样式。 </param>
    public TextElementBuilder(string text, TextStyle style = TextStyle.None)
    {
        Text = text;
        Style = style;
    }

    /// <summary>
    ///     设置此文本元素的文本。
    /// </summary>
    /// <param name="text"> 文本。 </param>
    /// <returns> 返回当前 <see cref="TextElementBuilder"/> 实例。 </returns>
    public TextElementBuilder WithText(string text)
    {
        Text = text;
        return this;
    }

    /// <summary>
    ///     设置此文本元素的样式。
    /// </summary>
    /// <param name="style"> 样式。 </param>
    /// <returns> 返回当前 <see cref="TextElementBuilder"/> 实例。 </returns>
    public TextElementBuilder WithStyle(TextStyle style)
    {
        Style = style;
        return this;
    }

    /// <summary>
    ///     设置此文本元素的样式。
    /// </summary>
    /// <param name="bold"> 加粗。 </param>
    /// <param name="italic"> 斜体。 </param>
    /// <param name="underline"> 下划线。 </param>
    /// <returns> 返回当前 <see cref="TextElementBuilder"/> 实例。 </returns>
    public TextElementBuilder WithStyle(bool? bold = null, bool? italic = null, bool? underline = null)
    {
        if (bold.HasValue) WithBold(bold.Value);
        if (italic.HasValue) WithItalic(italic.Value);
        if (underline.HasValue) WithUnderline(underline.Value);
        return this;
    }

    /// <summary>
    ///     设置此文本元素的加粗样式。
    /// </summary>
    /// <param name="bold"> 是否加粗。 </param>
    /// <returns> 返回当前 <see cref="TextElementBuilder"/> 实例。 </returns>
    public TextElementBuilder WithBold(bool bold = true)
    {
        Style = bold ? Style | TextStyle.Bold : Style & ~TextStyle.Bold;
        return this;
    }

    /// <summary>
    ///     设置此文本元素的斜体样式。
    /// </summary>
    /// <param name="italic"> 是否斜体。 </param>
    /// <returns> 返回当前 <see cref="TextElementBuilder"/> 实例。 </returns>
    public TextElementBuilder WithItalic(bool italic = true)
    {
        Style = italic ? Style | TextStyle.Italic : Style & ~TextStyle.Italic;
        return this;
    }

    /// <summary>
    ///     设置此文本元素的下划线样式。
    /// </summary>
    /// <param name="underline"> 是否下划线。 </param>
    /// <returns> 返回当前 <see cref="TextElementBuilder"/> 实例。 </returns>
    public TextElementBuilder WithUnderline(bool underline = true)
    {
        Style = underline ? Style | TextStyle.Underline : Style & ~TextStyle.Underline;
        return this;
    }
}
