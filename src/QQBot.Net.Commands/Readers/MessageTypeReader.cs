namespace QQBot.Commands;

/// <summary>
///     表示一个用于解析实现了 <see cref="QQBot.IMessage"/> 的对象的类型读取器。
/// </summary>
/// <typeparam name="T"> 要解析为的消息类型。 </typeparam>
public class MessageTypeReader<T> : TypeReader
    where T : class, IMessage
{
    /// <inheritdoc />
    public override async Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    {
        //By Id (1.0)
        if (await context.Channel.GetMessageAsync(input, CacheMode.CacheOnly).ConfigureAwait(false) is T msg)
            return TypeReaderResult.FromSuccess(msg);
        return TypeReaderResult.FromError(CommandError.ObjectNotFound, "Message not found.");
    }
}
