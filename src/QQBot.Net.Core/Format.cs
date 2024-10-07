using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

namespace QQBot;

/// <summary>
///     提供有关格式化的方法。
/// </summary>
public static class Format
{
    // Characters which need escaping
    private static readonly string[] SensitiveCharacters =
    [
        "\\", "*", "_", "~", "`", ".", ":", "/", ">", "|", "#"
    ];

    /// <summary>
    ///     返回一个使用粗体格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>*</c> 字符转义为 <c>\*</c>。
    /// </remarks>
    public static string Bold(string? text, bool sanitize = true) =>
        $"**{(sanitize ? Sanitize(text, "*") : text)}**";

    /// <summary>
    ///     返回一个使用斜体格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>*</c> 字符转义为 <c>\*</c>。
    /// </remarks>
    public static string Italics(string? text, bool sanitize = true) =>
        $"*{(sanitize ? text.Sanitize("*") : text)}*";

    /// <summary>
    ///     返回一个使用粗斜体格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>*</c> 字符转义为 <c>\*</c>。
    /// </remarks>
    public static string BoldItalics(string? text, bool sanitize = true) =>
        $"***{(sanitize ? text.Sanitize("*") : text)}***";

    /// <summary>
    ///     返回一个使用删除线格式的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c> 将会对文本中出现的所有 <c>~</c> 字符转义为 <c>\~</c>。
    /// </remarks>
    public static string Strikethrough(string? text, bool sanitize = true) =>
        $"~~{(sanitize ? text.Sanitize("~") : text)}~~";

    /// <summary>
    ///     返回格式化为 Markdown 链接的字符串。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的链接文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c>，将会对 URL 中出现的所有 <c>&lt;</c> 和 <c>&gt;</c> 字符分别转义为
    ///     <c>\&lt;</c> 和 <c>\&gt;</c>。
    /// </remarks>
    public static string Url(string url, bool sanitize = true) => $"<{url.Sanitize("<", ">")}>";

    /// <inheritdoc cref="QQBot.Format.Url(System.String,System.Boolean)" />
    public static string Url(Uri url, bool sanitize = true) => Url(url.OriginalString, sanitize);

    /// <summary>
    ///     返回格式化为 Markdown 链接的字符串。
    /// </summary>
    /// <param name="url"> 要链接到的 URL。 </param>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <param name="sanitize"> 是否要先对 <paramref name="text"/> 与 <paramref name="url"/> 中与当前格式化操作有冲突的字符进行转义。 </param>
    /// <returns> 获取格式化后的链接文本。 </returns>
    /// <remarks>
    ///     设置 <paramref name="sanitize"/> 为 <c>true</c>，将会对文本中出现的所有 <c>[</c> 和 <c>]</c> 字符分别转义为
    ///     <c>\[</c> 和 <c>\]</c>，并对 URL 中出现的所有 <c>(</c> 和 <c>)</c> 字符分别转义为 <c>\(</c> 和 <c>\)</c>。
    /// </remarks>
    public static string Url(string url, string text, bool sanitize = true) =>
        $"[{(sanitize ? text.Sanitize("[", "]") : text)}]({(sanitize ? url.Sanitize("(", ")") : url)})";

    /// <inheritdoc cref="QQBot.Format.Url(System.String,System.String,System.Boolean)" />
    public static string Url(Uri url, string text, bool sanitize = true) => Url(url.OriginalString, text, sanitize);

    /// <summary>
    ///     返回格式化为 Markdown 一级标题的字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    public static string H1(string text) => $"# {text}";

    /// <summary>
    ///     返回格式化为 Markdown 二级标题的字符串。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <returns> 获取格式化后的文本。 </returns>
    public static string H2(string text) => $"## {text}";

    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <returns> 获取格式化后的图片。 </returns>
    public static string Image(string url) => $"![]({url})";

    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="url"> 图片的 URL。 </param>
    /// <param name="alternative"> 图片的替代文本。 </param>
    /// <param name="size"> 图片的尺寸。 </param>
    /// <returns> 获取格式化后的图片。 </returns>
    public static string Image(string url, string alternative, Size? size = null) =>
        $"![{alternative}{(size.HasValue ? $" #{size.Value.Width}px #{size.Value.Height}px" : string.Empty)}]({url})";

    /// <inheritdoc cref="QQBot.Format.Image(System.String)" />
    public static string Image(Uri uri) => Image(uri.OriginalString);

    /// <inheritdoc cref="QQBot.Format.Image(System.String,System.String,System.Nullable{System.Drawing.Size})" />
    public static string Image(Uri uri, string alternative, Size? size = null) =>
        Image(uri.OriginalString, alternative, size);

    /// <summary>
    ///     获取一个 Markdown 格式的图片。
    /// </summary>
    /// <param name="attachment"> 图片的附件信息。 </param>
    /// <param name="size"> 图片的尺寸。 </param>
    /// <returns> 获取格式化后的图片。 </returns>
    public static string Image(FileAttachment attachment, Size? size = null)
    {
        if (attachment.Type is not AttachmentType.Image)
            throw new InvalidOperationException("The attachment is not an image.");
        if (attachment.Uri is null)
            throw new InvalidOperationException("The attachment does not have a valid URI.");
        return attachment.Filename is not null
            ? Image(attachment.Uri, attachment.Filename, size)
            : Image(attachment.Uri);
    }

    /// <summary>
    ///     获取有序列表的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="items"> 要格式化的列表项。 </param>
    /// <param name="indentLevel"> 列表项的缩进级别。 </param>
    /// <returns> 获取格式化后的列表。 </returns>
    public static string OrderedList(IEnumerable<string> items, int indentLevel = 0) => items
        .Select((item, index) => $"{new string(' ', indentLevel * 4)}{index + 1}. {item}")
        .Aggregate((current, next) => $"{current}\n{next}");

    /// <summary>
    ///     获取无序列表的 Markdown 格式化字符串。
    /// </summary>
    /// <param name="items"> 要格式化的列表项。 </param>
    /// <param name="indentLevel"> 列表项的缩进级别。 </param>
    /// <returns> 获取格式化后的列表。 </returns>
    public static string UnorderedList(IEnumerable<string> items, int indentLevel = 0) => items
        .Select(item => $"{new string(' ', indentLevel * 4)}- {item}")
        .Aggregate((current, next) => $"{current}\n{next}");

    /// <summary>
    ///     获取一个 Markdown 格式的块引用。
    /// </summary>
    /// <param name="text"> 要格式化的文本。 </param>
    /// <returns> 获取格式化后的块引用。 </returns>
    public static string BlockQuote(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return text;
        StringBuilder result = new();
        int startIndex = 0;
        int newLineIndex;
        do
        {
            newLineIndex = text.IndexOf('\n', startIndex);
            if (newLineIndex == -1)
                result.Append($"> {text[startIndex..]}");
            else
                result.Append($"> {text[startIndex..newLineIndex]}\n");
            startIndex = newLineIndex + 1;
        } while (newLineIndex != -1 && startIndex != text.Length);
        return result.ToString();
    }

    /// <summary>
    ///     获取一个 Markdown 格式的水平分割线。
    /// </summary>
    /// <returns> 获取格式化后的块引用。 </returns>
    public static string HorizontalRule() => "***";

    /// <summary>
    ///     获取一个 Markdown 格式的多行换行符。
    /// </summary>
    /// <param name="count"> 要获取的换行数量。 </param>
    /// <returns> 获取多行换行符。 </returns>
    public static string NewLine(int count = 1) =>
        count switch
        {
            < 1 => string.Empty,
            1 => "\n\n",
            > 1 => $"\n\n{string.Concat(Enumerable.Repeat("\u200B\n", count - 1))}"
        };

    /// <summary>
    ///     转义字符串，安全地转义任何 Markdown 序列。
    /// </summary>
    /// <param name="text"> 要转义的文本。 </param>
    /// <param name="sensitiveCharacters"> 要转义的字符。 </param>
    /// <returns> 获取转义后的文本。 </returns>
    /// <remarks>
    ///     如果未指定要转移的字符，则将使用默认的转义字符列表。默认的待转义字符包括：<br />
    ///     <c>\</c>、<c>*</c>、<c>~</c>、<c>`</c>、<c>:</c>、<c>-</c>、<c>]</c>、<c>)</c>、<c>&gt;</c>、<c>#</c>。
    /// </remarks>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? Sanitize(this string? text, params string[] sensitiveCharacters)
    {
        if (text is null) return null;
        string[] sensitiveChars = sensitiveCharacters.Length > 0 ? sensitiveCharacters : SensitiveCharacters;
        return sensitiveChars.Aggregate(text,
            (current, unsafeChar) => current.Replace(unsafeChar, $"\\{unsafeChar}"));
    }
}
