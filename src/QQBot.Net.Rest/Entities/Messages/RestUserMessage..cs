using System.Diagnostics;
using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个由用户发送的消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestUserMessage : RestMessage, IUserMessage
{
    internal RestUserMessage(BaseQQBotClient client, string id,
        IMessageChannel channel, IUser author, MessageSource source)
        : base(client, id, channel, author, source)
    {
    }

    internal static new RestUserMessage Create(BaseQQBotClient client,
        IMessageChannel channel, IUser author, ChannelMessage model)
    {
        RestUserMessage entity = new(client, model.Id, channel, author, MessageSource.User);
        entity.Update(model);
        return entity;
    }

    internal static new RestUserMessage Create(BaseQQBotClient client,
        IMessageChannel channel, IUser author,
        API.Rest.SendUserGroupMessageParams args, API.Rest.SendUserGroupMessageResponse model)
    {
        RestUserMessage entity = new(client, model.Id, channel, author, MessageSource.User);
        entity.Update(args, model);
        return entity;
    }

    /// <summary>
    ///     删除此对实体象及其所有子实体对象。
    /// </summary>
    /// <param name="hideTip"> 是否隐藏删除提示，仅在 <see cref="ITextChannel"/> 或 <see cref="IDMChannel"/> 内的消息支持。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    public Task DeleteAsync(bool? hideTip = null, RequestOptions? options = null) =>
        MessageHelper.DeleteAsync(this, Client, hideTip, options);

    /// <inheritdoc />
    public async Task PinAsync(RequestOptions? options = null)
    {
        if (Channel is not ITextChannel textChannel)
            throw new NotSupportedException("The message must be in a ITextChannel to pin.");
        await textChannel.PinMessageAsync(Id, options).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task UnpinAsync(RequestOptions? options = null)
    {
        if (Channel is not ITextChannel textChannel)
            throw new NotSupportedException("The message must be in a ITextChannel to unpin.");
        await textChannel.UnpinMessageAsync(Id, options).ConfigureAwait(false);
    }

    /// <inheritdoc />
    Task IDeletable.DeleteAsync(RequestOptions? options) => DeleteAsync(null, options);
}
