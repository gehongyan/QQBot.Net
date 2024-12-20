namespace QQBot;

/// <summary>
///     表示一个通用的消息。
/// </summary>
public interface IMessage : IEntity<string>
{
    /// <summary>
    ///     获取消息的来源子频道。
    /// </summary>
    IMessageChannel Channel { get; }

    /// <summary>
    ///     获取消息的发送者。
    /// </summary>
    IUser Author { get; }

    /// <summary>
    ///     获取消息的来源
    /// </summary>
    MessageSource Source { get; }

    /// <summary>
    ///     获取消息的内容。
    /// </summary>
    string Content { get; }

    /// <summary>
    ///     获取消息的创建时间。
    /// </summary>
    DateTimeOffset Timestamp { get; }

    /// <summary>
    ///     获取此消息中包含的所有附件。
    /// </summary>
    IReadOnlyCollection<IAttachment> Attachments { get; }

    /// <summary>
    ///     获取此消息是否提及了全体成员。
    /// </summary>
    bool? MentionedEveryone { get; }

    /// <summary>
    ///     获取此消息内包含的所有嵌入式内容。
    /// </summary>
    IReadOnlyCollection<IEmbed> Embeds { get; }

    /// <summary>
    ///     获取此消息中解析出的所有标签。
    /// </summary>
    IReadOnlyCollection<ITag> Tags { get; }

    /// <summary>
    ///     获此消息指定表情符号的所有回应用户。
    /// </summary>
    /// <param name="emote"> 要获取回应用户的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的可枚举集合，结果包含了所有回应用户的集合。 </returns>
    IAsyncEnumerable<IReadOnlyCollection<IGuildUser>> GetReactionUsersAsync(IEmote emote, RequestOptions? options = null);

    /// <summary>
    ///     向此消息添加一个回应。
    /// </summary>
    /// <param name="emote"> 要用于向此消息添加回应的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示添加添加异步操作的任务。 </returns>
    Task AddReactionAsync(IEmote emote, RequestOptions? options = null);

    /// <summary>
    ///     从此消息中移除一个当前用户的回应。
    /// </summary>
    /// <param name="emote"> 要从此消息移除的回应的表情符号。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步移除操作的任务。 </returns>
    Task RemoveReactionAsync(IEmote emote, RequestOptions? options = null);
}
