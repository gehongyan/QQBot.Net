namespace QQBot;

/// <summary>
///     表示一个富文本构建器。
/// </summary>
public class RichTextBuilder
{
    /// <summary>
    ///     初始化 <see cref="RichTextBuilder"/> 类的新实例。
    /// </summary>
    public RichTextBuilder()
    {
        Paragraphs = [];
    }

    /// <summary>
    ///     初始化 <see cref="RichTextBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="paragraphs"> 富文本的段落。 </param>
    public RichTextBuilder(IList<ParagraphBuilder> paragraphs)
    {
        Paragraphs = paragraphs;
    }

    /// <summary>
    ///     获取或设置此富文本的段落。
    /// </summary>
    public IList<ParagraphBuilder> Paragraphs { get; set; }

    /// <summary>
    ///     添加一个段落。
    /// </summary>
    /// <param name="paragraph"> 要添加的段落。 </param>
    /// <returns> 返回当前 <see cref="RichTextBuilder"/> 实例。 </returns>
    public RichTextBuilder AddParagraph(ParagraphBuilder paragraph)
    {
        Paragraphs.Add(paragraph);
        return this;
    }
}
