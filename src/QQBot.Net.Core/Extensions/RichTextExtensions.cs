namespace QQBot;

/// <summary>
///     提供 <see cref="RichText"/> 的扩展方法。
/// </summary>
public static class RichTextExtensions
{
    #region RichText Entity -> Buidler

    /// <summary>
    ///     将 <see cref="TextElement"/> 实体转换为具有相同属性的 <see cref="TextElementBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="TextElement"/> 实体。 </param>
    /// <returns> 具有相同属性的 <see cref="TextElementBuilder"/> 实体构建器。 </returns>
    public static TextElementBuilder ToBuilder(this TextElement entity) => new(entity.Text, entity.Style);

    /// <summary>
    ///     将 <see cref="ImageElement"/> 实体转换为具有相同必要属性的 <see cref="ImageElementBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="ImageElement"/> 实体。 </param>
    /// <returns> 具有相同必要属性的 <see cref="ImageElementBuilder"/> 实体构建器。 </returns>
    public static ImageElementBuilder ToBuilder(this ImageElement entity) => new(entity.Url, entity.Ratio);

    /// <summary>
    ///     将 <see cref="VideoElement"/> 实体转换为具有相同必要属性的 <see cref="VideoElementBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="VideoElement"/> 实体。 </param>
    /// <returns> 具有相同必要属性的 <see cref="VideoElementBuilder"/> 实体构建器。 </returns>
    public static VideoElementBuilder ToBuilder(this VideoElement entity) => new(entity.Url);

    /// <summary>
    ///     将 <see cref="UrlElement"/> 实体转换为具有相同属性的 <see cref="UrlElementBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="UrlElement"/> 实体。 </param>
    /// <returns> 具有相同属性的 <see cref="UrlElementBuilder"/> 实体构建器。 </returns>
    public static UrlElementBuilder ToBuilder(this UrlElement entity) => new(entity.Url, entity.Description);

    /// <summary>
    ///     将 <see cref="EmptyElement"/> 实体转换为具有相同属性的 <see cref="EmptyElementBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="EmptyElement"/> 实体。 </param>
    /// <returns> 具有相同属性的 <see cref="EmptyElementBuilder"/> 实体构建器。 </returns>
    public static EmptyElementBuilder ToBuilder(this EmptyElement entity) => new();

    /// <summary>
    ///     将 <see cref="IElement"/> 实体转换为具有相同属性的 <see cref="IElementBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="IElement"/> 实体。 </param>
    /// <returns> 具有相同属性的 <see cref="IElementBuilder"/> 实体构建器。 </returns>
    /// <exception cref="InvalidOperationException"> 未知的元素类型。 </exception>
    public static IElementBuilder ToBuilder(this IElement entity)
    {
        return entity switch
        {
            TextElement text => text.ToBuilder(),
            ImageElement image => image.ToBuilder(),
            VideoElement video => video.ToBuilder(),
            UrlElement url => url.ToBuilder(),
            EmptyElement empty => empty.ToBuilder(),
            _ => throw new InvalidOperationException("Unknown element type.")
        };
    }

    /// <summary>
    ///     将 <see cref="Paragraph"/> 实体转换为具有相同属性的 <see cref="ParagraphBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="Paragraph"/> 实体。 </param>
    /// <returns> 具有相同属性的 <see cref="ParagraphBuilder"/> 实体构建器。 </returns>
    public static ParagraphBuilder ToBuilder(this Paragraph entity) =>
        new([..entity.Elements.Select(x => x.ToBuilder())], entity.Alignment);

    /// <summary>
    ///     将 <see cref="RichText"/> 实体转换为具有相同属性的 <see cref="RichTextBuilder"/> 实体构建器。
    /// </summary>
    /// <param name="entity"> 要转换的 <see cref="RichText"/> 实体。 </param>
    /// <returns> 具有相同属性的 <see cref="RichTextBuilder"/> 实体构建器。 </returns>
    public static RichTextBuilder ToBuilder(this RichText entity) =>
        new([..entity.Paragraphs.Select(x => x.ToBuilder())]);

    #endregion
}
