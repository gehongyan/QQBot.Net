namespace QQBot;

/// <summary>
///     表示一个富文本段落构建器。
/// </summary>
public class ParagraphBuilder
{
    /// <summary>
    ///     初始化一个 <see cref="ParagraphBuilder"/> 类的新实例。
    /// </summary>
    public ParagraphBuilder()
    {
        Elements = [];
    }

    /// <summary>
    ///     初始化一个 <see cref="ParagraphBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="elements"> 段落的元素。 </param>
    /// <param name="alignment"> 段落的对齐方式。 </param>
    public ParagraphBuilder(IList<IElementBuilder> elements, TextAlignment? alignment = null)
    {
        Elements = elements;
        Alignment = alignment;
    }

    /// <summary>
    ///     获取或设置此段落的元素。
    /// </summary>
    public IList<IElementBuilder> Elements { get; set; }

    /// <summary>
    ///     获取此段落的对齐方式。
    /// </summary>
    public TextAlignment? Alignment { get; set; }

    /// <summary>
    ///     添加一个元素。
    /// </summary>
    /// <param name="element"> 要添加的元素。 </param>
    /// <returns> 返回当前 <see cref="ParagraphBuilder"/> 实例。 </returns>
    public ParagraphBuilder AddElement(IElementBuilder element)
    {
        Elements.Add(element);
        return this;
    }

    /// <summary>
    ///     设置此段落的对齐方式。
    /// </summary>
    /// <param name="alignment"> 要设置的对齐方式。 </param>
    /// <returns> 返回当前 <see cref="ParagraphBuilder"/> 实例。 </returns>
    public ParagraphBuilder WithAlignment(TextAlignment alignment)
    {
        Alignment = alignment;
        return this;
    }
}
