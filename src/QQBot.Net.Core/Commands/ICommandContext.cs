namespace QQBot.Commands;

/// <summary>
///     表示命令的上下文。这可能包括客户端、公会、子频道、用户和消息。
/// </summary>
public interface ICommandContext
{
    /// <summary>
    ///     获取命令执行时所使用的 <see cref="QQBot.IQQBotClient" />。
    /// </summary>
    IQQBotClient Client { get; }

    /// <summary>
    ///     获取命令执行所在的 <see cref="QQBot.IGuild" />。
    /// </summary>
    IGuild? Guild { get; }

    /// <summary>
    ///     获取命令执行所在的 <see cref="QQBot.IMessageChannel" />。
    /// </summary>
    IMessageChannel Channel { get; }

    /// <summary>
    ///     获取执行命令的 <see cref="QQBot.IUser" />。
    /// </summary>
    IUser User { get; }

    /// <summary>
    ///     获取命令解析的源 <see cref="QQBot.IUserMessage" />。
    /// </summary>
    IUserMessage Message { get; }
}
