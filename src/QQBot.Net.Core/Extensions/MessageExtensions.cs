namespace QQBot;

/// <summary>
///     提供用于各种消息实体的扩展方法。
/// </summary>
public static class MessageExtensions
{
    /// <summary>
    ///     向消息所属的频道回复文字消息。
    /// </summary>
    /// <param name="message"> 要回复的消息。 </param>
    /// <param name="content">Contents of the message.</param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    public static async Task<Cacheable<IUserMessage, string>> ReplyAsync(this IUserMessage message,
        string? content, RequestOptions? options = null) =>
        await message.Channel.SendMessageAsync(content, message.SourceIdentifier, options).ConfigureAwait(false);
}
