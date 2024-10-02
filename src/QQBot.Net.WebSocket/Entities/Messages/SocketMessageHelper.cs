using QQBot.Rest;

namespace QQBot.WebSocket;

internal static class SocketMessageHelper
{
    public static Attachment CreateAttachment(API.Gateway.Attachment model) =>
        new(GetAttachmentType(model.ContentType), model.Url)
        {
            Content = model.Content,
            Filename = model.Filename,
            Size = model.Size,
            ContentType = model.ContentType,
            Width = model.Width,
            Height = model.Height
        };

    public static Attachment CreateAttachment(API.MessageAttachment model) =>
        new(AttachmentType.File, model.Url);

    private static AttachmentType GetAttachmentType(string contentType)
    {
        if (contentType.StartsWith("image"))
            return AttachmentType.Image;
        if (contentType.StartsWith("video"))
            return AttachmentType.Video;
        if (contentType.StartsWith("voice"))
            return AttachmentType.Audio;
        return AttachmentType.File;
    }
}
