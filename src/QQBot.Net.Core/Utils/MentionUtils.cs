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
    ///     返回频道的 Markdown 格式化频道提及字符串。
    /// </summary>
    /// <param name="id"> 频道 ID。 </param>
    /// <returns> 格式化为 Markdown 的频道提及字符串。 </returns>
    public static string MentionChannel(ulong id) => $"<#{id}>";

    /// <summary>
    ///     返回频道的 Markdown 格式化频道提及字符串。
    /// </summary>
    /// <param name="channel"> 频道。 </param>
    /// <returns> 格式化为 Markdown 的频道提及字符串。 </returns>
    public static string MentionChannel(ITextChannel channel) => MentionChannel(channel.Id);
}

