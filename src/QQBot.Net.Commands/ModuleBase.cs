using QQBot.Commands.Builders;

namespace QQBot.Commands;

/// <summary>
///     表示一个模块基类。
/// </summary>
public abstract class ModuleBase : ModuleBase<ICommandContext>;

/// <summary>
///     表示一个模块基类。
/// </summary>
/// <typeparam name="T"> 模块的上下文类型。 </typeparam>
public abstract class ModuleBase<T> : IModuleBase
    where T : class, ICommandContext
{
    #region ModuleBase

    /// <summary>
    ///     获取此命令的上下文。
    /// </summary>
    public T Context { get; private set; } = null!; // 将由 SetContext 方法设置。

    /// <summary>
    ///     向消息所属的子频道回复消息。
    /// </summary>
    /// <param name="markdown"> 要回复的 Markdown 消息内容。 </param>
    /// <param name="attachment"> 要发送的文件附件。 </param>
    /// <param name="content"> 要发送的消息内容。 </param>
    /// <param name="embed"> 要发送的嵌入式消息内容。 </param>
    /// <param name="ark"> 要发送的模板消息内容。 </param>
    /// <param name="keyboard"> 要发送的按钮。 </param>
    /// <param name="messageReference"> 消息引用，用于回复消息。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步发送操作的任务。任务的结果包含所发送消息的可延迟加载的消息对象。 </returns>
    protected virtual async Task<IUserMessage> ReplyAsync(
        string? content = null, IMarkdown? markdown = null, FileAttachment? attachment = null,
        Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, RequestOptions? options = null) =>
        await Context.Message
            .ReplyAsync(content, markdown, attachment, embed, ark, keyboard, messageReference, options)
            .ConfigureAwait(false);

    /// <inheritdoc cref="QQBot.Commands.IModuleBase.BeforeExecuteAsync(QQBot.Commands.CommandInfo)" />
    protected virtual Task BeforeExecuteAsync(CommandInfo command) => Task.CompletedTask;

    /// <inheritdoc cref="QQBot.Commands.IModuleBase.BeforeExecute(QQBot.Commands.CommandInfo)" />
    protected virtual void BeforeExecute(CommandInfo command)
    {
    }

    /// <inheritdoc cref="QQBot.Commands.IModuleBase.AfterExecuteAsync(QQBot.Commands.CommandInfo)" />
    protected virtual Task AfterExecuteAsync(CommandInfo command) => Task.CompletedTask;

    /// <inheritdoc cref="QQBot.Commands.IModuleBase.AfterExecute(QQBot.Commands.CommandInfo)" />
    protected virtual void AfterExecute(CommandInfo command)
    {
    }

    /// <inheritdoc cref="QQBot.Commands.IModuleBase.OnModuleBuilding(QQBot.Commands.CommandService,QQBot.Commands.Builders.ModuleBuilder)" />
    protected virtual void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
    {
    }

    #endregion

    #region IModuleBase

    void IModuleBase.SetContext(ICommandContext context)
    {
        T? newValue = context as T;
        Context = newValue ?? throw new InvalidOperationException($"Invalid context type. Expected {typeof(T).Name}, got {context.GetType().Name}.");
    }

    Task IModuleBase.BeforeExecuteAsync(CommandInfo command) => BeforeExecuteAsync(command);
    void IModuleBase.BeforeExecute(CommandInfo command) => BeforeExecute(command);
    Task IModuleBase.AfterExecuteAsync(CommandInfo command) => AfterExecuteAsync(command);
    void IModuleBase.AfterExecute(CommandInfo command) => AfterExecute(command);

    void IModuleBase.OnModuleBuilding(CommandService commandService, ModuleBuilder builder) =>
        OnModuleBuilding(commandService, builder);

    #endregion
}
