namespace QQBot.Rest;

internal static class MessageHelper
{
    public static Attachment CreateAttachment(API.MessageAttachment model) =>
        new(AttachmentType.File, model.Url);
}
