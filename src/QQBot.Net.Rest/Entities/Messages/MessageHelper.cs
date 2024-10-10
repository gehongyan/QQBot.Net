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
}
