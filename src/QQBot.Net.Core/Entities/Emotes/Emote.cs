using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace QQBot;

/// <summary>
///     表示一个系统表情符号。
/// </summary>
public class Emote : IEmote
{
    // <faceType=3,faceId="338",ext="eyJ0ZXh0Ijoi5oiR5oOz5byA5LqGIn0="​>
    private static readonly Regex EmoteRegex = new("""^<faceType=(?<type>\d+),faceId="(?<id>\d*)",ext="(?<ext>[a-zA-Z0-9+/=]+)"\u200b?>$""", RegexOptions.Compiled);

    /// <inheritdoc />
    public string Id { get; }

    /// <summary>
    ///     获取表情符号的类型。
    /// </summary>
    public EmojiType Type { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <summary>
    ///     初始化一个 <see cref="Emote"/> 类的新实例。
    /// </summary>
    /// <param name="type"> 表情符号的类型。 </param>
    /// <param name="id"> 表情符号的 ID。 </param>
    /// <param name="name"> 表情符号的显示名称。 </param>
    public Emote(EmojiType type, string id, string name)
    {
        Type = type;
        Id = id;
        Name = name;
    }

    /// <summary>
    ///     初始化一个 <see cref="Emote"/> 类的新实例。
    /// </summary>
    /// <param name="emote"> 系统表情符号。 </param>
    /// <param name="name"> 表情符号名称。 </param>
    public Emote(Emotes.System emote, string? name = null)
    {
        Type = EmojiType.System;
        Id = ((int)emote).ToString();
        Name = name ?? Emotes.SystemNames.GetValueRefOrNullRef(emote);
    }

    /// <summary>
    ///     初始化一个 <see cref="Emote"/> 类的新实例。
    /// </summary>
    /// <param name="emote"> Emoji 表情符号。 </param>
    /// <param name="name"> 表情符号名称。 </param>
    public Emote(Emotes.Emoji emote, string? name = null)
    {
        Type = EmojiType.Emoji;
        Id = ((int)emote).ToString();
        Name = name ?? Emotes.EmojiNames.GetValueRefOrNullRef(emote);
    }

    /// <summary>
    ///     将一个表情符号的原始格式解析为一个 <see cref="Emote"/>。
    /// </summary>
    /// <param name="text"> 表情符号的原始格式。 </param>
    /// <returns> 解析出的 <see cref="Emote"/>。 </returns>
    /// <exception cref="FormatException"> 输入的文本不是有效的表情符号格式。 </exception>
    /// <example>
    ///     下面的示例演示了如何解析一个表情符号的原始格式：
    ///     <code language="cs">
    ///     Emote emote = Emote.Parse("&lt;faceType=1,faceId=\"12\",ext=\"eyJ0ZXh0Ijoi6LCD55quIn0=\"&gt;");
    ///     </code>
    /// </example>
    public static Emote Parse(string text)
    {
        if (EmoteRegex.Match(text) is not { Success: true } match)
            throw new FormatException("The input text is not a valid emote format.");
        int type = int.Parse(match.Groups["type"].Value);
        string id = match.Groups["id"].Value;
        string ext = match.Groups["ext"].Value;
        string json = Encoding.UTF8.GetString(Convert.FromBase64String(ext));
        JsonElement element = JsonSerializer.Deserialize<JsonElement>(json);
        string name = element.GetProperty("text").GetString() ?? string.Empty;
        return new Emote((EmojiType)type, id, name);
    }

    /// <summary>
    ///     尝试从一个表情符号的原始格式中解析出一个 <see cref="QQBot.Emote"/>。
    /// </summary>
    /// <param name="text"> 表情符号的原始格式。 </param>
    /// <param name="result"> 如果解析成功，则为解析出的 <see cref="QQBot.Emote"/>；否则为 <c>null</c>。 </param>
    /// <returns> 如果解析成功，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    /// <example>
    ///     下面的示例演示了如何解析一个表情符号的原始格式：
    ///     <code language="cs">
    ///     bool success = Emote.TryParse("&lt;faceType=1,faceId=\"12\",ext=\"eyJ0ZXh0Ijoi6LCD55quIn0=\"&gt;", out Emote? emote)
    ///     </code>
    /// </example>
    public static bool TryParse(string text, [NotNullWhen(true)] out Emote? result)
    {
        try
        {
            result = Parse(text);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}
