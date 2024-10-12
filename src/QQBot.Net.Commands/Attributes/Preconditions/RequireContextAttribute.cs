namespace QQBot.Commands;

/// <summary>
///     表示一个运行命令支持的上下文类型。
/// </summary>
[Flags]
public enum ContextType
{
    /// <summary>
    ///     命令可以在频道内执行。
    /// </summary>
    Guild = 0x01,

    /// <summary>
    ///     命令可以在频道私聊中执行。
    /// </summary>
    DM = 0x02,

    /// <summary>
    ///     命令可以在群聊中执行。
    /// </summary>
    Group = 0x04,

    /// <summary>
    ///     命令可以在用户单聊中执行。
    /// </summary>
    User = 0x08
}

/// <summary>
///     要求命令在指定的上下文类型中（例如在频道内、频道私聊中）执行。
/// </summary>
/// <example>
/// <code language="cs">
///     [Command("secret")]
///     [RequireContext(ContextType.DM | ContextType.Group)]
///     public Task PrivateOnlyAsync()
///     {
///         return ReplyTextAsync("shh, this command is a secret");
///     }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequireContextAttribute : PreconditionAttribute
{
    /// <summary>
    ///     获取命令执行所需的上下文类型。
    /// </summary>
    public ContextType Contexts { get; }

    /// <summary>
    ///     获取或设置错误消息。
    /// </summary>
    public override string? ErrorMessage { get; set; }

    /// <summary>
    ///     初始化一个 <see cref="RequireContextAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="contexts"> 命令执行所需的上下文类型。 </param>
    public RequireContextAttribute(ContextType contexts)
    {
        Contexts = contexts;
    }

    /// <inheritdoc />
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        bool isValid = false;
        if ((Contexts & ContextType.Guild) != 0)
            isValid = context.Channel is IGuildChannel;
        if ((Contexts & ContextType.DM) != 0)
            isValid = isValid || context.Channel is IDMChannel;
        if ((Contexts & ContextType.Group) != 0)
            isValid = isValid || context.Channel is IGroupChannel;
        if ((Contexts & ContextType.User) != 0)
            isValid = isValid || context.Channel is IUserChannel;

        PreconditionResult preconditionResult = isValid
            ? PreconditionResult.FromSuccess()
            : PreconditionResult.FromError(ErrorMessage ?? $"Invalid context for command; accepted contexts: {Contexts}.");
        return Task.FromResult(preconditionResult);
    }
}
