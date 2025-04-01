using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using QQBot.API;
using QQBot.API.Rest;
using QQBot.Net.Rest;

namespace QQBot.Rest;

internal static class ChannelHelper
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static int _messageSequenceIncrement;

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

    public static async Task DeleteAsync(IGuildChannel channel, BaseQQBotClient client, RequestOptions? options) =>
        await client.ApiClient.DeleteChannelAsync(channel.Id, options).ConfigureAwait(false);

    #endregion

    #region Send Messages

    public static async Task<IUserMessage> SendMessageAsync(
        IUserChannel channel, BaseQQBotClient client, string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options)
    {
        MediaFileInfo? mediaFileInfo = attachment.HasValue
            ? await EnsureUserGroupFileAttachmentAsync(client, channel, attachment.Value)
            : null;
        int messageSequence = CreateMessageSequence(client.MessageSequenceGenerationParameters,
            content, markdown, mediaFileInfo, embed, ark, keyboard, messageReference, passiveSource);
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
            MessageSequence = messageSequence
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
        int messageSequence = CreateMessageSequence(client.MessageSequenceGenerationParameters,
            content, markdown, mediaFileInfo, embed, ark, keyboard, messageReference, passiveSource);
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
            MessageSequence = messageSequence
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
        ChannelMessage response;
        try
        {
            response = await client.ApiClient.SendChannelMessageAsync(channel.Id, args, options).ConfigureAwait(false);
        }
        finally
        {
            if (needDispose && multipartFile.HasValue)
                await multipartFile.Value.Stream.DisposeAsync();
        }
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

    private static int CreateMessageSequence(MessageSequenceGenerationParameters generationParameters,
        string? content, IMarkdown? markdown, MediaFileInfo? mediaFileInfo,
        Embed? embed, Ark? ark, IKeyboard? keyboard, MessageReference? messageReference, IUserMessage? passiveSource)
    {
        int increment = Interlocked.Increment(ref _messageSequenceIncrement);
        int value = 1;
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.AutoIncrement))
            value = increment;
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Timestamp))
            value = HashCode.Combine(value, DateTimeOffset.Now.ToUnixTimeMilliseconds());
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Random))
            value = HashCode.Combine(value, RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Content))
            value = HashCode.Combine(value, content);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Markdown))
            value = HashCode.Combine(value, markdown);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.FileAttachment))
            value = HashCode.Combine(value, mediaFileInfo);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Embed))
            value = HashCode.Combine(value, embed);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Ark))
            value = HashCode.Combine(value, ark);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.Keyboard))
            value = HashCode.Combine(value, keyboard);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.MessageReference))
            value = HashCode.Combine(value, messageReference);
        if (generationParameters.HasFlag(MessageSequenceGenerationParameters.PassiveSource))
            value = HashCode.Combine(value, passiveSource);
        return value;
    }

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

    #region Get Messages

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

    #region Users

    public static async Task<int> CountOnlineUsersAsync(IVoiceChannel channel, BaseQQBotClient client, RequestOptions? options)
    {
        CountMediaChannelOnlineMembersResponse response = await client.ApiClient
            .CountMediaChannelOnlineMembersAsync(channel.Id, options).ConfigureAwait(false);
        return response.OnlineMembers;
    }

    public static async Task<int> CountOnlineUsersAsync(ILiveStreamChannel channel, BaseQQBotClient client, RequestOptions? options)
    {
        CountMediaChannelOnlineMembersResponse response = await client.ApiClient
            .CountMediaChannelOnlineMembersAsync(channel.Id, options).ConfigureAwait(false);
        return response.OnlineMembers;
    }

    #endregion

    #region Roles

    public static async Task<ChannelPermissions> GetPermissionsAsync(IGuildChannel channel, BaseQQBotClient client,
        IGuildMember user, RequestOptions? options)
    {
        API.ChannelPermissions model = await client.ApiClient
            .GetMemberPermissionsAsync(channel.Id, user.Id, options).ConfigureAwait(false);
        return new ChannelPermissions(ulong.Parse(model.Permissions));
    }

    public static async Task<ChannelPermissions> GetPermissionsAsync(IGuildChannel channel, BaseQQBotClient client,
        IRole role, RequestOptions? options)
    {
        API.ChannelPermissions model = await client.ApiClient
            .GetRolePermissionsAsync(channel.Id, role.Id, options).ConfigureAwait(false);
        return new ChannelPermissions(ulong.Parse(model.Permissions));
    }

    public static async Task ModifyPermissionsAsync(IGuildChannel channel, BaseQQBotClient client,
        IGuildMember user, OverwritePermissions permissions, RequestOptions? options)
    {
        ModifyMemberPermissionsParams args = new()
        {
            Add = permissions.AllowValue,
            Remove = permissions.DenyValue
        };
        await client.ApiClient.ModifyMemberPermissionsAsync(channel.Id, user.Id, args, options).ConfigureAwait(false);
    }

    public static async Task ModifyPermissionsAsync(IGuildChannel channel, BaseQQBotClient client,
        IRole role, OverwritePermissions permissions, RequestOptions? options)
    {
        ModifyRolePermissionsParams args = new()
        {
            Add = permissions.AllowValue,
            Remove = permissions.DenyValue
        };
        await client.ApiClient.ModifyRolePermissionsAsync(channel.Id, role.Id, args, options).ConfigureAwait(false);
    }

    #endregion

    #region API Permissions

    public static Task RequestApplicationPermissionAsync(ITextChannel channel, BaseQQBotClient client,
        string title, ApplicationPermission permission, RequestOptions? options) =>
        RequestApplicationPermissionAsync(channel, client, title, permission.Description,
            permission.Method, permission.Path, options);

    public static Task RequestApplicationPermissionAsync(ITextChannel channel, BaseQQBotClient client,
        string title, string description, HttpMethod method, string path, RequestOptions? options) =>
        RequestApplicationPermissionAsync(channel.GuildId, channel.Id, client, title, description, method, path, options);

    public static async Task RequestApplicationPermissionAsync(ulong guildId, ulong channelId, BaseQQBotClient client,
        string title, string description, HttpMethod method, string path, RequestOptions? options)
    {
        RequestApplicationGuildPermissionParams args = new()
        {
            GuildId = guildId,
            ChannelId = channelId,
            ApiIdentify = new ApiPermissionDemandIdentify
            {
                Method = method.Method,
                Path = path
            },
            Title = title,
            Description = description
        };
        await client.ApiClient.RequestApplicationGuildPermissionAsync(guildId, args, options);
    }

    #endregion

    #region Message Pins

    public static async Task<IReadOnlyCollection<ulong>> GetPinnedMessagesAsync(ITextChannel channel,
        BaseQQBotClient client, RequestOptions? options)
    {
        PinsMessage models = await client.ApiClient.GetPinedMessagesAsync(channel.Id, options).ConfigureAwait(false);
        return models.MessageIds;
    }

    public static Task PinMessageAsync(ITextChannel channel,
        BaseQQBotClient client, string messageId, RequestOptions? options) =>
        client.ApiClient.PinMessageAsync(channel.Id, messageId, options);

    public static Task UnpinMessageAsync(ITextChannel channel,
        BaseQQBotClient client, string messageId, RequestOptions? options) =>
        client.ApiClient.UnpinMessageAsync(channel.Id, messageId, options);

    #endregion

    #region Schedules

    public static async Task<IReadOnlyCollection<RestGuildSchedule>> GetSchedulesAsync(IScheduleChannel channel,
        BaseQQBotClient client, DateTimeOffset? since, RequestOptions? options)
    {
        IReadOnlyCollection<Schedule> models = await client.ApiClient
            .GetSchedulesAsync(channel.Id, since, options)
            .ConfigureAwait(false);
        return
        [
            ..models.Select(x => RestGuildSchedule.Create(client, channel, x,
                x.Creator.User is not null ? RestGuildMember.Create(client, channel.Guild, x.Creator.User, x.Creator) : null))
        ];
    }

    public static async Task<Schedule> GetScheduleAsync(IScheduleChannel channel,
        BaseQQBotClient client, ulong id, RequestOptions? options) =>
        await client.ApiClient.GetScheduleAsync(channel.Id, id, options).ConfigureAwait(false);

    public static async Task<RestGuildSchedule> CreateScheduleAsync(IScheduleChannel channel, BaseQQBotClient client,
        string name, DateTimeOffset startTime, DateTimeOffset endTime, string? description, IGuildChannel? jumpChannel,
        RemindType remindType, RequestOptions? options)
    {
        CreateScheduleParams args = new()
        {
            Schedule = new ScheduleParams
            {
                Name = name,
                Description = description,
                StartTimestamp = startTime,
                EndTimestamp = endTime,
                JumpChannelId = jumpChannel?.Id,
                RemindType = remindType
            }
        };
        Schedule model = await client.ApiClient.CreateScheduleAsync(channel.Id, args, options);
        return RestGuildSchedule.Create(client, channel, model,
            model.Creator.User is not null ? RestGuildMember.Create(client, channel.Guild, model.Creator.User, model.Creator) : null);
    }

    public static async Task<Schedule> ModifyAsync(IGuildSchedule schedule, BaseQQBotClient client,
        Action<ModifyGuildScheduleProperties> func, RequestOptions? options)
    {
        ModifyGuildScheduleProperties props = new()
        {
            Name = schedule.Name,
            Description = schedule.Description,
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,
            JumpChannelId = schedule.JumpChannelId,
            RemindType = schedule.RemindType
        };
        func(props);
        ModifyScheduleParams args = new()
        {
            Schedule = new ScheduleParams
            {
                Name = props.Name,
                Description = props.Description,
                StartTimestamp = props.StartTime,
                EndTimestamp = props.EndTime,
                JumpChannelId = props.JumpChannelId,
                RemindType = props.RemindType
            }
        };
        return await client.ApiClient
            .ModifyScheduleAsync(schedule.Channel.Id, schedule.Id, args, options)
            .ConfigureAwait(false);
    }

    public static Task DeleteAsync(IGuildSchedule schedule, BaseQQBotClient client, RequestOptions? options) =>
        client.ApiClient.DeleteScheduleAsync(schedule.Channel.Id, schedule.Id, options);

    #endregion

    #region Audio Control

    public static Task JoinAsync(IVoiceChannel channel, BaseQQBotClient client, RequestOptions? options) =>
        client.ApiClient.JoinMicrophoneAsync(channel.Id, options);

    public static Task LeaveAsync(IVoiceChannel channel, BaseQQBotClient client, RequestOptions? options) =>
        client.ApiClient.LeaveMicrophoneAsync(channel.Id, options);

    public static Task PlayAsync(IVoiceChannel channel, BaseQQBotClient client,
        string url, string displayText, RequestOptions? options) =>
        ControlAudioAsync(channel, client, AudioStatus.Start, url, displayText, options);

    public static Task PauseAsync(IVoiceChannel channel, BaseQQBotClient client, RequestOptions? options) =>
        ControlAudioAsync(channel, client, AudioStatus.Pause, null, null, options);

    public static Task ResumeAsync(IVoiceChannel channel, BaseQQBotClient client, RequestOptions? options) =>
        ControlAudioAsync(channel, client, AudioStatus.Resume, null, null, options);

    public static Task StopAsync(IVoiceChannel channel, BaseQQBotClient client, RequestOptions? options) =>
        ControlAudioAsync(channel, client, AudioStatus.Stop, null, null, options);

    private static Task ControlAudioAsync(IVoiceChannel channel, BaseQQBotClient client,
        AudioStatus status, string? audioUrl, string? text, RequestOptions? options)
    {
        AudioControl args = new()
        {
            Status = status,
            AudioUrl = audioUrl,
            Text = text
        };
        return client.ApiClient.ControlAudioAsync(channel.Id, args, options);
    }

    #endregion

    #region Forum

    public static async Task<IReadOnlyCollection<RestThread>> GetThreadsAsync(IForumChannel channel,
        BaseQQBotClient client, RequestOptions? options)
    {
        GetForumThreadsResponse response = await client.ApiClient
            .GetForumThreadsAsync(channel.Id, options)
            .ConfigureAwait(false);
        return [..response.Threads.Select(x => RestThread.Create(client, channel, x.AuthorId, x.ThreadInfo))];
    }

    public static async Task<API.Thread> GetThreadAsync(IForumChannel channel,
        BaseQQBotClient client, string id, RequestOptions? options)
    {
        GetForumThreadResponse response = await client.ApiClient
            .GetForumThreadAsync(channel.Id, id, options)
            .ConfigureAwait(false);
        return response.Thread;
    }

    public static async Task CreateThreadAsync(IForumChannel channel, BaseQQBotClient client,
        string title, ThreadTextType textType, string content, RequestOptions? options)
    {
        CreateForumThreadParams args = new()
        {
            Title = title,
            Format = textType,
            Content = content
        };
        await client.ApiClient.CreateForumThreadAsync(channel.Id, args, options).ConfigureAwait(false);
    }

    public static Task CreateThreadAsync(IForumChannel channel, BaseQQBotClient client,
        string title, RichTextBuilder content, RequestOptions? options)
    {
        string json = JsonSerializer.Serialize(content.ToModel(), _serializerOptions);
        return CreateThreadAsync(channel, client, title, ThreadTextType.Json, json, options);
    }

    public static Task DeleteThreadAsync(IForumChannel channel, BaseQQBotClient client,
        string id, RequestOptions? options) =>
        client.ApiClient.DeleteForumThreadAsync(channel.Id, id, options);

    #endregion
}
