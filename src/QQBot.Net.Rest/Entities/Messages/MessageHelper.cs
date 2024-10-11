using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class MessageHelper
{
    public static Attachment CreateAttachment(API.MessageAttachment model) =>
        new(AttachmentType.File, model.Url);

    public static async Task<IUser> GetAuthorAsync(BaseQQBotClient client, IGuild? guild, API.User model)
    {
        IUser? author = guild is not null
            ? await guild.GetUserAsync(model.Id, CacheMode.CacheOnly)
            : null;
        return author ?? RestGuildUser.Create(client, model);
    }

    public static async Task DeleteAsync(IUserMessage message, BaseQQBotClient client, bool? hideTip, RequestOptions? options)
    {
        switch (message.Channel)
        {
            case IUserChannel userChannel:
                await client.ApiClient.DeleteUserMessageAsync(userChannel.Id, message.Id, options).ConfigureAwait(false);
                break;
            case IGroupChannel groupChannel:
                await client.ApiClient.DeleteGroupMessageAsync(groupChannel.Id, message.Id, options).ConfigureAwait(false);
                break;
            case ITextChannel textChannel:
                await client.ApiClient
                    .DeleteChannelMessageAsync(textChannel.Id, message.Id,
                        new DeleteChannelMessageParams { HideTip = hideTip }, options)
                    .ConfigureAwait(false);
                break;
            case IDMChannel dmChannel:
                await client.ApiClient
                    .DeleteDirectMessageAsync(dmChannel.Id, message.Id,
                        new DeleteDirectMessageParams { HideTip = hideTip }, options)
                    .ConfigureAwait(false);
                break;
            default:
                throw new NotSupportedException("Unsupported channel type.");
        }
    }
}
