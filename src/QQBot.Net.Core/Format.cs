using System.Collections.Frozen;
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

    private static readonly FrozenDictionary<string, string> EscapingMap = new Dictionary<string, string>
    {
        { "&", "&amp;" },
        { "<", "&lt;" },
        { ">", "&gt;" },
    }.ToFrozenDictionary();

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
        $"*{(sanitize ? Sanitize(text, "*") : text)}*";

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
        $"***{(sanitize ? Sanitize(text, "*") : text)}***";

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
        $"~~{(sanitize ? Sanitize(text, "~") : text)}~~";

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
    public static string Url(string url, bool sanitize = true) => $"<{Sanitize(url, "<", ">")}>";

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
        $"[{(sanitize ? Sanitize(text, "[", "]") : text)}]({(sanitize ? Sanitize(url, "(", ")") : url)})";

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
    ///     获取一个格式化的命令。
    /// </summary>
    /// <param name="text"> 用户点击后直接发送的文本。 </param>
    /// <returns> 获取格式化后的命令。 </returns>
    public static string Command(string text) => $"""<qqbot-cmd-enter text="{text}" />""";

    /// <summary>
    ///     获取一个格式化的命令。
    /// </summary>
    /// <param name="text"> 用户点击后直接发送的文本。 </param>
    /// <param name="displayText"> 用户在消息内看到的文本。 </param>
    /// <param name="reference"> 插入输入框时是否带消息原文回复引用。 </param>
    /// <returns> 获取格式化后的命令。 </returns>
    public static string Command(string text, string displayText, bool reference) =>
        $"""<qqbot-cmd-enter text="{text}" show="{displayText}" reference="{reference}" />""";

    /// <summary>
    ///     获取一个格式化的表情符号。
    /// </summary>
    /// <param name="emote"> 表情符号。 </param>
    /// <returns> 获取格式化后的表情符号。 </returns>
    public static string Emote(Emotes.System emote) => Emote((int)emote);

    /// <summary>
    ///     获取一个格式化的表情符号。
    /// </summary>
    /// <param name="emoteId"> 表情符号 ID。 </param>
    /// <returns> 获取格式化后的表情符号。 </returns>
    public static string Emote(int emoteId) => $"<emoji:{emoteId}>";

    /// <summary>
    ///     转义字符串，安全地转义任何 Markdown 序列。
    /// </summary>
    /// <param name="text"> 要转义的文本。 </param>
    /// <param name="sensitiveCharacters"> 要转义的字符。 </param>
    /// <returns> 获取转义后的文本。 </returns>
    /// <remarks>
    ///     如果未指定要转移的字符，则将使用默认的转义字符列表。默认的待转义字符包括：<br />
    ///     <c>\</c>、<c>*</c>、<c>~</c>、<c>`</c>、<c>:</c>、<c>-</c>、<c>]</c>、<c>)</c>、<c>&gt;</c>、<c>#</c>。
    ///     此方法会在每个待转义字符前添加一个零宽空格字符（U+200B）。此操作会使内嵌格式提及失效。 <br />
    ///     例如，用户发送用户提及 <c>@someone</c>，网关下传 <c>&lt;@!4937680016579989979&gt;</c>，经过此方法转义的结果为
    ///     <c>&lt;@!4937680016579989979\u200B&gt;</c>，发送给用户后，用户将看到 <c>&lt;@!4937680016579989979&gt;</c>。 <br />
    ///     例如，用户发送 <c>&lt;@!4937680016579989979&gt;</c>，网关下传
    ///     <c>&amp;lt;@!4937680016579989979&amp;gt;</c>，经过此方法转义的结果为
    ///     <c>&amp;lt;@!4937680016579989979&amp;\u200Bgt;</c>，发送给用户后，用户将看到
    ///     <c>&lt;@!4937680016579989979&gt;</c>。
    /// </remarks>
    /// <seealso cref="QQBot.Format.Escape(System.String)"/>
    [return: NotNullIfNotNull(nameof(text))]
    public static string? Sanitize(string? text, params string[] sensitiveCharacters)
    {
        if (text is null) return null;
        string[] sensitiveChars = sensitiveCharacters.Length > 0 ? sensitiveCharacters : SensitiveCharacters;
        return sensitiveChars.Aggregate(text,
            (current, unsafeChar) => current.Replace(unsafeChar, $"\u200B{unsafeChar}"));
    }

    /// <summary>
    ///     转义字符串，将原始文本中的内嵌格式转义为转义字符。
    /// </summary>
    /// <param name="text"> 要转义的文本。 </param>
    /// <returns> 转义后的文本。 </returns>
    /// <remarks>
    ///     此方法会将文本中的 <c>&amp;</c>、<c>&lt;</c> 和 <c>&gt;</c> 字符分别转义为 <c>&amp;amp;</c>、<c>&amp;lt;</c> 和 <c>&amp;gt;</c>。
    ///     此操作会保留网关或 API 返回的文本内容在用户侧的显示形式。 <br />
    ///     例如，用户发送用户提及 <c>@someone</c>，网关下传 <c>&lt;@!4937680016579989979&gt;</c>，经过此方法转义的结果为
    ///     <c>&amp;lt;@!4937680016579989979&amp;gt;</c>，发送给用户后，用户将看到 <c>&lt;@!4937680016579989979&gt;</c>。 <br />
    ///     例如，用户发送 <c>&lt;@!4937680016579989979&gt;</c>，网关下传
    ///     <c>&amp;lt;@!4937680016579989979&amp;gt;</c>，经过此方法转义的结果为
    ///     <c>&amp;amp;lt;@!4937680016579989979&amp;amp;gt;</c>，发送给用户后，用户将看到
    ///     <c>&amp;lt;@!4937680016579989979&amp;gt;</c>。
    /// </remarks>
    /// <seealso cref="QQBot.Format.Sanitize(System.String,System.String[])"/>
    public static string? Escape(string? text)
    {
        if (text is null) return null;
        return EscapingMap.Aggregate(text, (current, pair) => current.Replace(pair.Key, pair.Value));
    }

    /// <summary>
    ///     反转义字符串，将转义的内嵌格式还原为原始文本。
    /// </summary>
    /// <param name="text"> 要反转义的文本。 </param>
    /// <returns> 反转义后的文本。 </returns>
    /// <remarks>
    ///     此方法会将文本中的 <c>&amp;amp;</c>、<c>&amp;lt;</c> 和 <c>&amp;gt;</c> 字符分别反转义为
    ///     <c>&amp;</c>、<c>&lt;</c> 和 <c>&gt;</c>。此操作可以使用户发送的内嵌格式生效为实际的提及。 <br />
    ///     例如，用户发送 <c>&lt;@!4937680016579989979&gt;</c>，网关下传
    ///     <c>&amp;lt;@!4937680016579989979&amp;gt;</c>，经过此方法反转义的结果为
    ///     <c>&lt;@!4937680016579989979&gt;</c>，发送给用户后，用户将看到 <c>@someone</c>。
    /// </remarks>
    public static string? Unescape(string? text)
    {
        if (text is null) return null;
        return EscapingMap.Aggregate(text, (current, pair) => current.Replace(pair.Value, pair.Key));
    }
}
