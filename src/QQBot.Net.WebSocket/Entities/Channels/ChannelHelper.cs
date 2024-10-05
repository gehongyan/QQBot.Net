using QQBot.API;
using QQBot.API.Rest;
using QQBot.Rest;

namespace QQBot.WebSocket;

internal static class ChannelHelper
{
    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(IUserChannel channel,
        BaseQQBotClient client, string? content, MessageSourceIdentifier? passiveSource, RequestOptions? options)
    {
        SendUserGroupMessageParams args = new()
        {
            Content = content,
            MessageType = MessageType.Text,
            Markdown = null,
            Keyboard = null,
            Ark = null,
            MediaFileInfo = null,
            EventId = null,
            MessageId = passiveSource?.MessageId,
            MessageSequence = CreateMessageSequence(content, passiveSource)
        };
        SendUserGroupMessageResponse response = await client.ApiClient
            .SendUserMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        return CreateCacheable(response.Id);
    }

    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(IGroupChannel channel,
        BaseQQBotClient client, string? content, MessageSourceIdentifier? passiveSource, RequestOptions? options)
    {
        SendUserGroupMessageParams args = new()
        {
            Content = content,
            MessageType = MessageType.Text,
            Markdown = null,
            Keyboard = null,
            Ark = null,
            MediaFileInfo = null,
            EventId = null,
            MessageId = passiveSource?.MessageId,
            MessageSequence = CreateMessageSequence(content, passiveSource)
        };
        SendUserGroupMessageResponse response = await client.ApiClient
            .SendGroupMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        return CreateCacheable(response.Id);
    }

    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(ITextChannel channel,
        BaseQQBotClient client, string? content, MessageSourceIdentifier? passiveSource, RequestOptions? options)
    {
        SendChannelMessageParams args = new()
        {
            Content = content,
            Embed = null,
            Markdown = null,
            Ark = null,
            MessageReference = null,
            Image = null,
            EventId = null,
            MessageId = passiveSource?.MessageId,
        };
        ChannelMessage response = await client.ApiClient
            .SendChannelMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        return CreateCacheable(response.Id);
    }

    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(IDMChannel channel,
        BaseQQBotClient client, string? content, MessageSourceIdentifier? passiveSource, RequestOptions? options)
    {
        SendChannelMessageParams args = new()
        {
            Content = content,
            Embed = null,
            Markdown = null,
            Ark = null,
            MessageReference = null,
            Image = null,
            EventId = null,
            MessageId = passiveSource?.MessageId,
        };
        ChannelMessage response = await client.ApiClient
            .SendDirectMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        return CreateCacheable(response.Id);
    }

    private static int CreateMessageSequence(string? content, MessageSourceIdentifier? passiveSource) =>
        Math.Abs(HashCode.Combine(content, passiveSource?.Dispatch, passiveSource?.MessageId));

    private static Cacheable<IUserMessage, string> CreateCacheable(string id) =>
        new(null, id, false, () => Task.FromResult<IUserMessage?>(null));
}
