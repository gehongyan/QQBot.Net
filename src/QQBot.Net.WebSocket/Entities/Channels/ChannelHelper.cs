using QQBot.API;
using QQBot.API.Rest;
using QQBot.Net.Rest;
using QQBot.Rest;

namespace QQBot.WebSocket;

internal static class ChannelHelper
{
    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(IUserChannel channel,
        BaseQQBotClient client, string? content, FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        MediaFileInfo? mediaFileInfo = attachment.HasValue
            ? await EnsureUserGroupFileAttachmentAsync(client, channel, attachment.Value)
            : null;
        SendUserGroupMessageParams args = new()
        {
            Content = content,
            MessageType = GetMessageType(content, attachment, embed, ark),
            Markdown = null,
            Keyboard = null,
            Ark = ark?.ToModel(),
            MediaFileInfo = mediaFileInfo.HasValue
                ? new API.Rest.MediaFileInfo { FileInfo = mediaFileInfo.Value.FileInfo }
                : null,
            MessageReference = messageReference?.ToModel(),
            EventId = null, // Using MessageId to identify the event is enough
            MessageId = passiveSource?.Id,
            MessageSequence = CreateMessageSequence(content, embed, messageReference, passiveSource)
        };
        SendUserGroupMessageResponse response = await client.ApiClient
            .SendUserMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        return CreateCacheable(response.Id);
    }

    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(IGroupChannel channel,
        BaseQQBotClient client, string? content, FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        MediaFileInfo? mediaFileInfo = attachment.HasValue
            ? await EnsureUserGroupFileAttachmentAsync(client, channel, attachment.Value)
            : null;
        SendUserGroupMessageParams args = new()
        {
            Content = content,
            MessageType = GetMessageType(content, attachment, embed, ark),
            Markdown = null,
            Keyboard = null,
            Ark = ark?.ToModel(),
            MediaFileInfo = mediaFileInfo.HasValue
                ? new API.Rest.MediaFileInfo { FileInfo = mediaFileInfo.Value.FileInfo }
                : null,
            MessageReference = messageReference?.ToModel(),
            EventId = null, // Using MessageId to identify the event is enough
            MessageId = passiveSource?.Id,
            MessageSequence = CreateMessageSequence(content, embed, messageReference, passiveSource)
        };
        SendUserGroupMessageResponse response = await client.ApiClient
            .SendGroupMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        return CreateCacheable(response.Id);
    }

    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(ITextChannel channel,
        BaseQQBotClient client, string? content, FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        (string? uri, MultipartFile? multipartFile, bool needDispose) = attachment.HasValue
            ? EnsureChannelFileAttachmentAsync(attachment.Value)
            : (null, null, false);
        SendChannelMessageParams args = new()
        {
            Content = content,
            Embed = embed?.ToModel(),
            Markdown = null,
            Ark = ark?.ToModel(),
            MessageReference = messageReference?.ToModel(),
            Image = uri,
            FileImage = multipartFile,
            EventId = null, // Using MessageId to identify the event is enough
            MessageId = passiveSource?.Id,
        };
        ChannelMessage response = await client.ApiClient
            .SendChannelMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        if (needDispose && multipartFile.HasValue)
            await multipartFile.Value.Stream.DisposeAsync();
        return CreateCacheable(response.Id);
    }

    public static async Task<Cacheable<IUserMessage, string>> SendMessageAsync(IDMChannel channel,
        BaseQQBotClient client, string? content, FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        (string? uri, MultipartFile? multipartFile, bool needDispose) = attachment.HasValue
            ? EnsureChannelFileAttachmentAsync(attachment.Value)
            : (null, null, false);
        SendChannelMessageParams args = new()
        {
            Content = content,
            Embed = embed?.ToModel(),
            Markdown = null,
            Ark = ark?.ToModel(),
            MessageReference = messageReference?.ToModel(),
            Image = uri,
            FileImage = multipartFile,
            EventId = null, // Using MessageId to identify the event is enough
            MessageId = passiveSource?.Id,
        };
        ChannelMessage response = await client.ApiClient
            .SendDirectMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        if (needDispose && multipartFile.HasValue)
            await multipartFile.Value.Stream.DisposeAsync();
        return CreateCacheable(response.Id);
    }

    private static MessageType GetMessageType(string? content, FileAttachment? attachment, Embed? embed, Ark? ark)
    {
        if (attachment is not null) return MessageType.Media;
        if (embed is not null) return MessageType.Embed;
        if (ark is not null) return MessageType.Ark;
        return MessageType.Text;
    }

    private static int CreateMessageSequence(string? content, Embed? embed,
        MessageReference? messageReference, IUserMessage? passiveSource) =>
        HashCode.Combine(content, embed, messageReference, passiveSource?.Id) & int.MaxValue;

    private static Cacheable<IUserMessage, string> CreateCacheable(string id) =>
        new(null, id, false, () => Task.FromResult<IUserMessage?>(null));

    private static async Task<MediaFileInfo?> EnsureUserGroupFileAttachmentAsync(BaseQQBotClient client, IMessageChannel channel, FileAttachment attachment)
    {
        switch (attachment.Mode)
        {
            case CreateAttachmentMode.FilePath:
            case CreateAttachmentMode.Stream:
                throw new NotSupportedException("CreateAttachmentMode.FilePath and CreateAttachmentMode.Stream are not supported when sent to IUserChannel or IGroupChannel.");
            case CreateAttachmentMode.Uri:
                if (attachment.Uri is null)
                    throw new InvalidOperationException("The Uri in the FileAttachment must not be null when creating a FileAttachment with CreateAttachmentMode.Uri.");
                switch (channel)
                {
                    case IUserChannel userChannel:
                    {
                        if (attachment.UserMediaFileInfo?.HasExpired is false)
                            return attachment.UserMediaFileInfo.Value;
                        SendAttachmentResponse response = await client.ApiClient.CreateUserAttachmentAsync(
                            userChannel.Id, new SendAttachmentParams
                            {
                                FileType = attachment.Type,
                                Url = attachment.Uri.OriginalString,
                                ServerSendMessage = false
                            });
                        attachment.UserMediaFileInfo = new MediaFileInfo
                        {
                            FileId = response.FileUuid,
                            AttachmentType = attachment.Type,
                            CreatedAt = DateTimeOffset.Now,
                            LifeTime = TimeSpan.FromSeconds(response.TimeToLive),
                            FileInfo = response.FileInfo
                        };
                        return attachment.UserMediaFileInfo.Value;
                    }
                    case IGroupChannel groupChannel:
                    {
                        if (attachment.GroupMediaFileInfo?.HasExpired is false)
                            return attachment.GroupMediaFileInfo.Value;
                        SendAttachmentResponse response = await client.ApiClient.CreateGroupAttachmentAsync(
                            groupChannel.Id, new SendAttachmentParams
                            {
                                FileType = attachment.Type,
                                Url = attachment.Uri.OriginalString,
                                ServerSendMessage = false
                            });
                        attachment.GroupMediaFileInfo = new MediaFileInfo
                        {
                            FileId = response.FileUuid,
                            AttachmentType = attachment.Type,
                            CreatedAt = DateTimeOffset.Now,
                            LifeTime = TimeSpan.FromSeconds(response.TimeToLive),
                            FileInfo = response.FileInfo
                        };
                        return attachment.GroupMediaFileInfo.Value;
                    }
                    default:
                        throw new NotSupportedException("Unsupported channel type.");
                }
            case CreateAttachmentMode.MediaFileInfo:
                switch (channel)
                {
                    case IUserChannel:
                        if (!attachment.UserMediaFileInfo.HasValue)
                            throw new InvalidOperationException("The FileAttachment must have a UserMediaFileInfo when sending an attachment to a IUserChannel.");
                        if (attachment.UserMediaFileInfo.Value.HasExpired is true)
                            throw new InvalidOperationException("The UserMediaFileInfo in the FileAttachment has expired.");
                        return attachment.UserMediaFileInfo.Value;
                    case IGroupChannel:
                        if (!attachment.GroupMediaFileInfo.HasValue)
                            throw new InvalidOperationException("The FileAttachment must have a GroupMediaFileInfo when sending an attachment to a IGroupChannel.");
                        if (attachment.GroupMediaFileInfo.Value.HasExpired is true)
                            throw new InvalidOperationException("The GroupMediaFileInfo in the FileAttachment has expired.");
                        return attachment.GroupMediaFileInfo.Value;
                    default:
                        throw new NotSupportedException("Unsupported channel type.");
                }
            default:
                throw new NotSupportedException("Unsupported attachment mode.");
        }
    }

    private static (string?, MultipartFile?, bool needDispose) EnsureChannelFileAttachmentAsync(FileAttachment attachment)
    {
        if (attachment.Type != AttachmentType.Image)
            throw new NotSupportedException("Only image attachments are supported when sending to a ITextChannel or IDMChannel.");
        switch (attachment.Mode)
        {
            case CreateAttachmentMode.FilePath:
            {
                if (attachment.FilePath is null)
                    throw new InvalidOperationException("The FilePath in the FileAttachment must not be null when creating a FileAttachment with CreateAttachmentMode.FilePath.");
                FileStream stream = File.OpenRead(attachment.FilePath);
                return (null, new MultipartFile(stream, attachment.Filename), true);
            }
            case CreateAttachmentMode.Stream:
            {
                if (attachment.Stream is null)
                    throw new InvalidOperationException("The Stream in the FileAttachment must not be null when creating a FileAttachment with CreateAttachmentMode.Stream.");
                if (attachment.IsDisposed)
                    throw new InvalidOperationException("The stream in the FileAttachment has been disposed.");
                return (null, new MultipartFile(attachment.Stream, attachment.Filename), false);
            }
            case CreateAttachmentMode.Uri:
                return (attachment.Uri?.OriginalString, null, false);
            case CreateAttachmentMode.MediaFileInfo:
                throw new NotSupportedException("The FileAttachment with CreateAttachmentMode.MediaFileInfo is not supported when sending to a ITextChannel or IDMChannel.");
            default:
                throw new NotSupportedException("Unsupported attachment mode.");
        }
    }
}
