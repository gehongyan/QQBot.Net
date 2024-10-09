using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

namespace QQBot;

/// <summary>
///     表示一个用于创建 <see cref="MarkdownText"/> 实例的构建器。
/// </summary>
public class MarkdownTextBuilder : IMarkdownBuilder, IEquatable<MarkdownTextBuilder>
{
    [MemberNotNullWhen(true, nameof(_textBuilder))]
    [MemberNotNullWhen(false, nameof(_simpleText))]
    private bool UseStringBuilder { get; set; }

    private string? _simpleText;
    private StringBuilder? _textBuilder;

    /// <summary>
    ///     获取或设置 Markdown 文本内容。
    /// </summary>
    public string Text
    {
        get => UseStringBuilder ? _textBuilder.ToString() : _simpleText;
        set
        {
            if (UseStringBuilder)
            {
                _textBuilder.Clear();
                _textBuilder.Append(value);
            }
            else
                _simpleText = value;
        }
    }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownTextBuilder"/> 类的新实例。
    /// </summary>
    public MarkdownTextBuilder()
    {
        Text = string.Empty;
    }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownTextBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="text"> Markdown 文本内容。 </param>
    public MarkdownTextBuilder(string text)
    {
        Text = text;
    }

    /// <summary>
    ///     初始化一个 <see cref="MarkdownTextBuilder"/> 类的新实例。
    /// </summary>
    /// <param name="textBuilder"> 用于构建 Markdown 文本内容的 <see cref="StringBuilder"/> 实例。 </param>
    public MarkdownTextBuilder(StringBuilder textBuilder)
    {
        UseStringBuilder = true;
        _textBuilder = textBuilder;
    }

    [MemberNotNull(nameof(_textBuilder))]
    private void EnsureStringBuilder()
    {
        if (UseStringBuilder) return;
        _textBuilder = new StringBuilder(_simpleText);
        UseStringBuilder = true;
        _simpleText = null;
    }

    /// <inheritdoc cref="IMarkdownBuilder.Build" />
    public MarkdownText Build() => new(Text);

    /// <inheritdoc />
    IMarkdown IMarkdownBuilder.Build() => new MarkdownText(Text);

    /// <inheritdoc />
    public bool Equals(MarkdownTextBuilder? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Text == other.Text;
    }

    /// <summary>
    ///     将指定的文本追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"></param>
    public void Append(string text)
    {
        EnsureStringBuilder();
        _textBuilder.Append(text);
    }

    /// <summary>
    ///     将指定的文本格式化为粗体并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Bold(System.String,System.Boolean)"/>
    public void AppendBold(string text, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Bold(text, sanitize));
    }

    /// <summary>
    ///     将指定的文本格式化为斜体并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Italics(System.String,System.Boolean)"/>
    public void AppendItalics(string text, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Italics(text, sanitize));
    }

    /// <summary>
    ///     将指定的文本格式化为粗斜体并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.BoldItalics(System.String,System.Boolean)"/>
    public void AppendBoldItalics(string text, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.BoldItalics(text, sanitize));
    }

    /// <summary>
    ///     将指定的文本格式化为删除线并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Strikethrough(System.String,System.Boolean)"/>
    public void AppendStrikethrough(string text, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Strikethrough(text, sanitize));
    }

    /// <summary>
    ///     将指定的 URL 格式化为链接并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Url(System.String,System.Boolean)"/>
    public void AppendUrl(string url, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Url(url, sanitize));
    }

    /// <summary>
    ///     将指定的 URL 格式化为链接并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Url(System.Uri,System.Boolean)"/>
    public void AppendUrl(Uri url, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Url(url, sanitize));
    }

    /// <summary>
    ///     将指定的 URL 格式化为链接并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="text"> 要显示的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 与 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Url(System.String,System.String,System.Boolean)"/>
    public void AppendUrl(string url, string text, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Url(url, text, sanitize));
    }

    /// <summary>
    ///     将指定的 URL 格式化为链接并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="text"> 要显示的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 与 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <seealso cref="QQBot.Format.Url(System.Uri,System.String,System.Boolean)"/>
    public void AppendUrl(Uri url, string text, bool sanitize = true)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Url(url, text, sanitize));
    }

    /// <summary>
    ///     将指定的文本格式化为一级标题并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <seealso cref="QQBot.Format.H1(System.String)"/>
    public void AppendH1(string text)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.H1(text));
    }

    /// <summary>
    ///     将指定的文本格式化为二级标题并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <seealso cref="QQBot.Format.H2(System.String)"/>
    public void AppendH2(string text)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.H2(text));
    }

    /// <summary>
    ///     将指定的图片 URL 格式化为图片并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <seealso cref="QQBot.Format.Image(System.String)"/>
    public void AppendImage(string url)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Image(url));
    }

    /// <summary>
    ///     将指定的图片 URL 格式化为图片并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <seealso cref="QQBot.Format.Image(System.Uri)"/>
    public void AppendImage(Uri url)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Image(url));
    }

    /// <summary>
    ///     将指定的图片 URL 格式化为图片并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <param name="alternative"> 图片的替代文本。 </param>
    /// <param name="size"> 图片的尺寸。 </param>
    /// <seealso cref="QQBot.Format.Image(System.String,System.String,System.Nullable{System.Drawing.Size})"/>
    public void AppendImage(string url, string alternative, Size? size = null)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Image(url, alternative, size));
    }

    /// <summary>
    ///     将指定的图片 URL 格式化为图片并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <param name="alternative"> 图片的替代文本。 </param>
    /// <param name="size"> 图片的尺寸。 </param>
    /// <seealso cref="QQBot.Format.Image(System.Uri,System.String,System.Nullable{System.Drawing.Size})"/>
    public void AppendImage(Uri url, string alternative, Size? size = null)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Image(url, alternative, size));
    }

    /// <summary>
    ///     将指定的图片附件格式化为图片并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="attachment"> 图片的附件信息。 </param>
    /// <param name="size"> 图片的尺寸。 </param>
    /// <seealso cref="QQBot.Format.Image(QQBot.FileAttachment,System.Nullable{System.Drawing.Size})"/>
    public void AppendImage(FileAttachment attachment, Size? size = null)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.Image(attachment, size));
    }

    /// <summary>
    ///     将指定的有序列表格式化为 Markdown 列表并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="items"> 要追加的列表项。 </param>
    /// <param name="indentLevel"> 列表项的缩进级别。 </param>
    /// <seealso cref="QQBot.Format.OrderedList(System.Collections.Generic.IEnumerable{System.String},System.Int32)"/>
    public void AppendOrderedList(IEnumerable<string> items, int indentLevel = 0)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.OrderedList(items, indentLevel));
    }

    /// <summary>
    ///     将指定的无序列表格式化为 Markdown 列表并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="items"> 要追加的列表项。 </param>
    /// <param name="indentLevel"> 列表项的缩进级别。 </param>
    /// <seealso cref="QQBot.Format.UnorderedList(System.Collections.Generic.IEnumerable{System.String},System.Int32)"/>
    public void AppendUnorderedList(IEnumerable<string> items, int indentLevel = 0)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.UnorderedList(items, indentLevel));
    }

    /// <summary>
    ///     将指定的文本格式化为块引用并追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="text"> 要追加的文本。 </param>
    /// <seealso cref="QQBot.Format.BlockQuote(System.String)"/>
    public void AppendBlockQuote(string text)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.BlockQuote(text));
    }

    /// <summary>
    ///     将水平分割线追加到当前构建器的文本内容中。
    /// </summary>
    /// <seealso cref="QQBot.Format.HorizontalRule"/>
    public void AppendHorizontalRule()
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.HorizontalRule());
    }

    /// <summary>
    ///     将指定数量的换行符追加到当前构建器的文本内容中。
    /// </summary>
    /// <param name="count"> 要追加的换行数量。 </param>
    /// <seealso cref="Format.NewLine(System.Int32)"/>
    public void AppendNewLine(int count = 1)
    {
        EnsureStringBuilder();
        _textBuilder.Append(Format.NewLine(count));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MarkdownTextBuilder builder && Equals(builder);

    /// <summary>
    ///     确定两个 <see cref="MarkdownTextBuilder"/> 实例是否相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator ==(MarkdownTextBuilder? left, MarkdownTextBuilder? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    ///     确定两个 <see cref="MarkdownTextBuilder"/> 实例是否不相等。
    /// </summary>
    /// <param name="left"> 要比较的第一个实例。 </param>
    /// <param name="right"> 要比较的第二个实例。 </param>
    /// <returns> 如果两个实例不相等，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool operator !=(MarkdownTextBuilder? left, MarkdownTextBuilder? right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();
}
