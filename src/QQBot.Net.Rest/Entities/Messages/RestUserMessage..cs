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

    /// <inheritdoc />
    public Task DeleteAsync(bool? hideTip = null, RequestOptions? options = null) =>
        MessageHelper.DeleteAsync(this, Client, hideTip, options);
}
