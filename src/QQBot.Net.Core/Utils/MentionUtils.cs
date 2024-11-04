using System.Globalization;
using System.Text;

namespace QQBot;

/// <summary>
///     提供一组用于生成与解析提及标签的辅助方法。
/// </summary>
public static class MentionUtils
{
    private const char SanitizeChar = '\x200B';

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
    public static string MentionChannel(ulong id) => MentionChannel(id.ToIdString());

    internal static string MentionChannel(string id) => $"<#{id}>";

    /// <summary>
    ///     返回子频道的 Markdown 格式化子频道提及字符串。
    /// </summary>
    /// <param name="channel"> 子频道。 </param>
    /// <returns> 格式化为 Markdown 的子频道提及字符串。 </returns>
    public static string MentionChannel(ITextChannel channel) => MentionChannel(channel.Id);

    /// <summary>
    ///     返回提及全体成员的 Markdown 格式化全体成员提及字符串。
    /// </summary>
    /// <returns> 格式化为 Markdown 的全体成员提及字符串。 </returns>
    public static string MentionEveryone => "<qqbot-at-everyone />";

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
    ///     尝试解析提供的用户提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的用户提及字符串。 </param>
    /// <param name="userId"> 解析出的用户 ID。 </param>
    /// <returns> 是否成功解析。 </returns>
    public static bool TryParseUser(string text, out ulong userId)
    {
        // <qqbot-at-user id="id" />
        ReadOnlySpan<char> textSpan = text.AsSpan();
        if (textSpan.StartsWith("<qqbot-at-user id=\"", StringComparison.Ordinal)
            && textSpan[19..].IndexOf('\"') is var end and > 0
            && ulong.TryParse(textSpan[19..(19 + end)], NumberStyles.None, CultureInfo.InvariantCulture, out userId))
            return true;

        // <@id> or <!@id>
        if (textSpan is ['<', '@', ..var middleSpan, '>'])
        {
            ReadOnlySpan<char> valueSpan = middleSpan.StartsWith("!") ? middleSpan[1..] : middleSpan;
            if (ulong.TryParse(valueSpan, NumberStyles.None, CultureInfo.InvariantCulture, out userId))
                return true;
        }

        userId = 0;
        return false;
    }

    /// <summary>
    ///     解析提供的子频道提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的子频道提及字符串。 </param>
    /// <returns> 解析出的子频道 ID。 </returns>
    /// <exception cref="ArgumentException"> 当提供的字符串不是有效的提及格式时引发。 </exception>
    public static ulong ParseChannel(string text)
    {
        if (TryParseChannel(text, out ulong id))
            return id;
        throw new ArgumentException(message: "Invalid mention format.", paramName: nameof(text));
    }

    /// <summary>
    ///     尝试解析提供的子频道提及字符串。
    /// </summary>
    /// <param name="text"> 要解析的子频道提及字符串。 </param>
    /// <param name="channelId"> 解析出的子频道 ID。 </param>
    /// <returns> 是否成功解析。 </returns>
    public static bool TryParseChannel(string text, out ulong channelId)
    {
        // <#id>
        ReadOnlySpan<char> textSpan = text.AsSpan();
        if (textSpan is ['<', '#', ..var middleSpan, '>']
            && ulong.TryParse(middleSpan, NumberStyles.None, CultureInfo.InvariantCulture, out channelId))
            return true;
        channelId = 0;
        return false;
    }

    internal static string Resolve(IMessage msg, int startIndex,
        TagHandling userHandling, TagHandling channelHandling,
        TagHandling everyoneHandling, TagHandling emojiHandling)
    {
        StringBuilder text = new(msg.Content[startIndex..]);
        IReadOnlyCollection<ITag> tags = msg.Tags;
        int indexOffset = -startIndex;

        foreach (ITag tag in tags)
        {
            if (tag.Index < startIndex)
                continue;

            string newText;
            switch (tag.Type)
            {
                case TagType.UserMention:
                    if (userHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveUserMention(tag, userHandling);
                    break;
                case TagType.ChannelMention:
                    if (channelHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveChannelMention(tag, channelHandling);
                    break;
                case TagType.EveryoneMention:
                    if (everyoneHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveEveryoneMention(tag, everyoneHandling);
                    break;
                case TagType.Emoji:
                    if (emojiHandling == TagHandling.Ignore)
                        continue;
                    newText = ResolveEmoji(tag, emojiHandling);
                    break;
                // TODO: Command
                default:
                    newText = string.Empty;
                    break;
            }
            text.Remove(tag.Index + indexOffset, tag.Length);
            text.Insert(tag.Index + indexOffset, newText);
            indexOffset += newText.Length - tag.Length;
        }
        return text.ToString();
    }

    internal static string ResolveUserMention(ITag tag, TagHandling mode)
    {
        if (tag.Value is not IGuildUser user)
            return string.Empty;
        IGuildMember? member = user as IGuildMember;
        return mode switch
        {
            TagHandling.Name => $"@{member?.Nickname ?? user.Username}",
            TagHandling.NameNoPrefix => $"{member?.Nickname ?? user.Username}",
            TagHandling.Sanitize => MentionUser($"{SanitizeChar}{tag.Key}"),
            _ => string.Empty
        };
    }

    internal static string ResolveChannelMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        if (tag.Value is not IGuildChannel channel)
            return string.Empty;
        return mode switch
        {
            TagHandling.Name => $"#{channel.Name}",
            TagHandling.NameNoPrefix => $"{channel.Name}",
            TagHandling.Sanitize => MentionChannel($"{SanitizeChar}{tag.Key}"),
            _ => string.Empty
        };
    }

    internal static string ResolveEveryoneMention(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        return mode switch
        {
            TagHandling.Name => "@全体成员",
            TagHandling.NameNoPrefix => "全体成员",
            TagHandling.Sanitize => $"@{SanitizeChar}all",
            _ => string.Empty
        };
    }

    internal static string ResolveEmoji(ITag tag, TagHandling mode)
    {
        if (mode == TagHandling.Remove)
            return string.Empty;
        if (tag.Value is not Emote emoji)
            return string.Empty;

        //Remove if its name contains any bad chars (prevents a few tag exploits)
        if (emoji.Name.Any(c => !char.IsLetterOrDigit(c) && c != '_' && c != '-'))
            return string.Empty;

        return mode switch
        {
            TagHandling.Name => $":{emoji.Name}:",
            TagHandling.NameNoPrefix => $"{emoji.Name}",
            TagHandling.Sanitize => $"(emj){SanitizeChar}{emoji.Name}(emj)[{SanitizeChar}{emoji.Id}]",
            _ => ""
        };
    }
}
