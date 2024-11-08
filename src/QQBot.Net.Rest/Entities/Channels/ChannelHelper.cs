using QQBot.API;
using QQBot.API.Rest;
using QQBot.Net.Rest;

namespace QQBot.Rest;

internal static class ChannelHelper
{
    #region Channels

    public static async Task UpdateAsync(RestGuildChannel channel, RequestOptions? options)
    {
        Channel model = await channel.Client.ApiClient.GetChannelAsync(channel.Id, options).ConfigureAwait(false);
        channel.Update(model);
    }

    public static async Task<Channel> ModifyAsync(IGuildChannel channel, BaseQQBotClient client,
        Action<ModifyGuildChannelProperties> func, RequestOptions? options)
    {
        ModifyGuildChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(ITextChannel channel, BaseQQBotClient client,
        Action<ModifyTextChannelProperties> func, RequestOptions? options)
    {
        ModifyTextChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            CategoryId = props.CategoryId,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(IVoiceChannel channel, BaseQQBotClient client,
        Action<ModifyVoiceChannelProperties> func, RequestOptions? options)
    {
        ModifyVoiceChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            CategoryId = props.CategoryId,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(ILiveStreamChannel channel, BaseQQBotClient client,
        Action<ModifyLiveStreamChannelProperties> func, RequestOptions? options)
    {
        ModifyLiveStreamChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            CategoryId = props.CategoryId,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(IApplicationChannel channel, BaseQQBotClient client,
        Action<ModifyApplicationChannelProperties> func, RequestOptions? options)
    {
        ModifyApplicationChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            CategoryId = props.CategoryId,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(IForumChannel channel, BaseQQBotClient client,
        Action<ModifyForumChannelProperties> func, RequestOptions? options)
    {
        ModifyForumChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            CategoryId = props.CategoryId,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(IScheduleChannel channel, BaseQQBotClient client,
        Action<ModifyScheduleChannelProperties> func, RequestOptions? options)
    {
        ModifyScheduleChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            CategoryId = props.CategoryId,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static async Task<Channel> ModifyAsync(ICategoryChannel channel, BaseQQBotClient client,
        Action<ModifyCategoryChannelProperties> func, RequestOptions? options)
    {
        ModifyCategoryChannelProperties props = new();
        func(props);
        ModifyChannelParams args = new()
        {
            Name = props.Name,
            Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission
        };
        return await client.ApiClient.ModifyChannelAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    #endregion

    #region Send Message

    public static async Task<IUserMessage> SendMessageAsync(
        IUserChannel channel, BaseQQBotClient client, string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        MediaFileInfo? mediaFileInfo = attachment.HasValue
            ? await EnsureUserGroupFileAttachmentAsync(client, channel, attachment.Value)
            : null;
        SendUserGroupMessageParams args = new()
        {
            Content = content,
            MessageType = InferMessageType(content, markdown, attachment, embed, ark, keyboard),
            Markdown = markdown?.ToModel(),
            Keyboard = keyboard?.ToModel(),
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
        return CreateMessageEntity(client, channel, args, response);
    }

    public static async Task<IUserMessage> SendMessageAsync(
        IGroupChannel channel, BaseQQBotClient client, string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        MediaFileInfo? mediaFileInfo = attachment.HasValue
            ? await EnsureUserGroupFileAttachmentAsync(client, channel, attachment.Value)
            : null;
        SendUserGroupMessageParams args = new()
        {
            Content = content,
            MessageType = InferMessageType(content, markdown, attachment, embed, ark, keyboard),
            Markdown = markdown?.ToModel(),
            Keyboard = keyboard?.ToModel(),
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
        return CreateMessageEntity(client, channel, args, response);
    }

    public static async Task<IUserMessage> SendMessageAsync(
        ITextChannel channel, BaseQQBotClient client, string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        (string? uri, MultipartFile? multipartFile, bool needDispose) = attachment.HasValue
            ? EnsureChannelFileAttachmentAsync(attachment.Value)
            : (null, null, false);
        SendChannelMessageParams args = new()
        {
            Content = content,
            Embed = embed?.ToModel(),
            Markdown = markdown?.ToModel(),
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
        return CreateMessageEntity(client, channel, response);
    }

    public static async Task<IUserMessage> SendMessageAsync(
        IDMChannel channel, BaseQQBotClient client, string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        (string? uri, MultipartFile? multipartFile, bool needDispose) = attachment.HasValue
            ? EnsureChannelFileAttachmentAsync(attachment.Value)
            : (null, null, false);
        SendChannelMessageParams args = new()
        {
            Content = content,
            Embed = embed?.ToModel(),
            Markdown = markdown?.ToModel(),
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
        return CreateMessageEntity(client, channel, response);
    }

    private static MessageType InferMessageType(string? content, IMarkdown? markdown, FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard)
    {
        if (markdown is not null || keyboard is not null) return MessageType.Markdown;
        if (attachment is not null) return MessageType.Media;
        if (embed is not null) return MessageType.Embed;
        if (ark is not null) return MessageType.Ark;
        if (content is not null) return MessageType.Text;
        throw new InvalidOperationException("No message content is provided.");
    }

    private static int CreateMessageSequence(string? content, Embed? embed,
        MessageReference? messageReference, IUserMessage? passiveSource) =>
        HashCode.Combine(content, embed, messageReference, passiveSource?.Id) & int.MaxValue;

    private static RestUserMessage CreateMessageEntity(BaseQQBotClient client, IMessageChannel channel,
        SendUserGroupMessageParams args, SendUserGroupMessageResponse model)
    {
        if (client.CurrentUser is null)
            throw new InvalidOperationException("The client must have a current user.");
        return RestUserMessage.Create(client, channel, client.CurrentUser, args, model);
    }

    private static RestUserMessage CreateMessageEntity(BaseQQBotClient client, IMessageChannel channel, ChannelMessage model)
    {
        if (client.CurrentUser is null)
            throw new InvalidOperationException("The client must have a current user.");
        return RestUserMessage.Create(client, channel, client.CurrentUser, model);
    }

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

    #endregion

    #region GetMessage

    public static async Task<RestUserMessage> GetMessageAsync<T>(T channel, BaseQQBotClient client,
        string id, RequestOptions? options)
        where T : IMessageChannel, IEntity<ulong>
    {
        ulong channelId = (channel as IEntity<ulong>).Id;
        IGuild? guild = (channel as IGuildChannel)?.Guild;
        ChannelMessage model = await client.ApiClient.GetMessageAsync(channelId, id, options).ConfigureAwait(false);
        IUser author = await MessageHelper.GetAuthorAsync(client, guild, model.Author);
        return RestUserMessage.Create(client, channel, author, model);
    }

    #endregion
}
