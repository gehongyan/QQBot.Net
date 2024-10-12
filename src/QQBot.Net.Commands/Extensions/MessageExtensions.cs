namespace QQBot.Commands;

/// <summary>
///     提供用于 <see cref="QQBot.IUserMessage" /> 与命令相关的扩展方法。
/// </summary>
public static class MessageExtensions
{
    /// <summary>
    ///     修剪消息的前导空格。
    /// </summary>
    /// <param name="msg"> 要修剪的消息。 </param>
    /// <param name="argPos"> 开始修剪的位置。 </param>
    public static void TrimStart(this IUserMessage msg, ref int argPos)
    {
        string text = msg.Content[argPos..];
        while (text.Length > 0 && char.IsWhiteSpace(text[0]))
        {
            argPos += 1;
            text = text[1..];
        }
    }

    /// <summary>
    ///     获取消息是否以提供的字符开头。
    /// </summary>
    /// <param name="msg"> 要检查的消息。 </param>
    /// <param name="c"> 要检查的前导字符。 </param>
    /// <param name="argPos"> 开始检查的位置。 </param>
    /// <returns> 如果消息以指定的字符开头，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool HasCharPrefix(this IUserMessage msg, char c, ref int argPos)
    {
        string text = msg.Content[argPos..];

        if (!string.IsNullOrEmpty(text) && text[0] == c)
        {
            argPos += 1;
            return true;
        }

        return false;
    }

    /// <summary>
    ///     获取消息是否以提供的字符串开头。
    /// </summary>
    /// <param name="msg"> 要检查的消息。 </param>
    /// <param name="str"> 要检查的前导字符。 </param>
    /// <param name="argPos"> 开始检查的位置。 </param>
    /// <param name="comparisonType"> 字符串比较模式。 </param>
    /// <returns> 如果消息以指定的字符串开头，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool HasStringPrefix(this IUserMessage msg, string str,
        ref int argPos, StringComparison comparisonType = StringComparison.Ordinal)
    {
        string text = msg.Content[argPos..];

        if (!string.IsNullOrEmpty(text) && text.StartsWith(str, comparisonType))
        {
            argPos += str.Length;
            return true;
        }

        return false;
    }

    /// <summary>
    ///     获取消息是否以提供的用户提及开头。
    /// </summary>
    /// <param name="msg"> 要检查的消息。 </param>
    /// <param name="user"> 要检查的用户。 </param>
    /// <param name="argPos"> 开始检查的位置。 </param>
    /// <returns> 如果消息以指定的用户提及开头，则为 <c>true</c>；否则为 <c>false</c>。 </returns>
    public static bool HasMentionPrefix(this IUserMessage msg, IUser user, ref int argPos)
    {
        string text = msg.Content[argPos..];
        if (string.IsNullOrEmpty(text) || text.Length <= 3 || text[0] != '<' || text[1] != '@')
            return false;

        int endPos = text.IndexOf('>');
        if (endPos == -1)
            return false;
        if (text.Length < endPos + 2 || text[endPos + 1] != ' ')
            return false; //Must end in "> "

        if (!MentionUtils.TryParseUser(text.Substring(0, endPos + 1), out ulong userId))
            return false;
        if (userId.ToIdString() == user.Id)
        {
            argPos += endPos + 2;
            return true;
        }
        return false;
    }
}
