using System.Globalization;

namespace QQBot;

/// <summary>
///     提供一组用于生成与解析提及标签的辅助方法。
/// </summary>
public static class MentionUtils
{
    /// <summary>
    ///     返回用户的 Markdown 格式化用户提及字符串。
    /// </summary>
    /// <param name="id"> 用户 ID。 </param>
    /// <returns> 格式化为 Markdown 的用户提及字符串。 </returns>
    public static string MentionUser(string id) => $"""<qqbot-at-user id="{id}" />""";

    /// <summary>
    ///     返回用户的 Markdown 格式化用户提及字符串。
    /// </summary>
    /// <param name="user"> 用户。 </param>
    /// <returns> 格式化为 Markdown 的用户提及字符串。 </returns>
    public static string MentionUser(IUser user) => MentionUser(user.Id);

    /// <summary>
    ///     返回子频道的 Markdown 格式化子频道提及字符串。
    /// </summary>
    /// <param name="id"> 子频道 ID。 </param>
    /// <returns> 格式化为 Markdown 的子频道提及字符串。 </returns>
    public static string MentionChannel(ulong id) => $"<#{id}>";

    /// <summary>
    ///     返回子频道的 Markdown 格式化子频道提及字符串。
    /// </summary>
    /// <param name="channel"> 子频道。 </param>
    /// <returns> 格式化为 Markdown 的子频道提及字符串。 </returns>
    public static string MentionChannel(ITextChannel channel) => MentionChannel(channel.Id);

    /// <summary>
    ///     解析提供的用户提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的用户提及字符串。 </param>
    /// <exception cref="ArgumentException"> 当提供的字符串不是有效的提及格式时引发。 </exception>
    public static ulong ParseUser(string text)
    {
        if (TryParseUser(text, out ulong id))
            return id;
        throw new ArgumentException(message: "Invalid mention format.", paramName: nameof(text));
    }

    /// <summary>
    ///     Tries to parse a provided user mention string.
    /// </summary>
    /// <param name="text">The user mention.</param>
    /// <param name="userId">The UserId of the user.</param>
    public static bool TryParseUser(string text, out ulong userId)
    {
        if (text is ['<', '@', ..var middlePart, '>'])
        {
            string valuePart = middlePart.StartsWith('!') ? middlePart[1..] : middlePart;
            if (ulong.TryParse(valuePart, NumberStyles.None, CultureInfo.InvariantCulture, out userId))
                return true;
        }

        userId = 0;
        return false;
    }
}
