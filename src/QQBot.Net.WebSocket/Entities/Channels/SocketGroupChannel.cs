using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的群组子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGroupChannel : SocketChannel, IGroupChannel, ISocketMessageChannel
{
    /// <inheritdoc cref="QQBot.IGroupChannel.Id" />
    public new Guid Id { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<SocketMessage> CachedMessages => [];

    /// <inheritdoc />
    internal SocketGroupChannel(QQBotSocketClient client, Guid id)
        : base(client, id.ToIdString())
    {
        Id = id;
    }

    internal static SocketGroupChannel Create(QQBotSocketClient client, ClientState state, Guid id)
    {
        SocketGroupChannel channel = new(client, id);
        return channel;
    }

    internal void AddMessage(SocketMessage message) { }

    private string DebuggerDisplay => $"Unknown ({Id}, Group)";

    #region Messages

    /// <inheritdoc cref="QQBot.IMessageChannel.SendMessageAsync(System.String,IMarkdown,System.Nullable{QQBot.FileAttachment},QQBot.Embed,QQBot.Ark,QQBot.IKeyboard,QQBot.MessageReference,QQBot.IUserMessage,QQBot.RequestOptions)" />
    public Task<IUserMessage> SendMessageAsync(string? content = null, IMarkdown? markdown = null,
        FileAttachment? attachment = null, Embed? embed = null, Ark? ark = null, IKeyboard? keyboard = null,
        MessageReference? messageReference = null, IUserMessage? passiveSource = null, RequestOptions? options = null) =>
        ChannelHelper.SendMessageAsync(this, Client, content, markdown, attachment, embed, ark, keyboard,
            messageReference, passiveSource, null, options);

    /// <inheritdoc />
    public Task<MediaUploadResult> UploadMediaAsync(MediaUploadSource source,
        IProgress<MediaUploadProgress>? progress = null, MediaUploadOptions? uploadOptions = null,
        RequestOptions? options = null) =>
        MediaUploadHelper.UploadAsync(this, Client, source, progress, uploadOptions, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.DeleteMessageAsync(System.String,QQBot.RequestOptions)" />
    public Task DeleteMessageAsync(string messageId, RequestOptions? options = null) =>
        ChannelHelper.DeleteGroupMessageAsync(this, Client, messageId, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.DeleteMessageAsync(QQBot.IUserMessage,QQBot.RequestOptions)" />
    public Task DeleteMessageAsync(IUserMessage message, RequestOptions? options = null) =>
        DeleteMessageAsync(message.Id, options);

    #endregion

    #region Group

    /// <inheritdoc cref="QQBot.IGroupChannel.GetInfoAsync(QQBot.RequestOptions)" />
    public Task<GroupInfo> GetInfoAsync(RequestOptions? options = null) =>
        GroupHelper.GetInfoAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.GetBotStateAsync(QQBot.RequestOptions)" />
    public Task<GroupBotState> GetBotStateAsync(RequestOptions? options = null) =>
        GroupHelper.GetBotStateAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.GetMembersAsync(QQBot.RequestOptions)" />
    public IAsyncEnumerable<IReadOnlyCollection<IGroupMember>> GetMembersAsync(RequestOptions? options = null) =>
        GroupHelper.GetMembersAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.GetMemberAsync(System.Guid,QQBot.RequestOptions)" />
    public Task<IGroupMember?> GetMemberAsync(Guid id, RequestOptions? options = null) =>
        GroupHelper.GetMemberAsync(this, Client, id, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.GetJoinRequestsAsync(QQBot.RequestOptions)" />
    public IAsyncEnumerable<IReadOnlyCollection<GroupJoinRequest>> GetJoinRequestsAsync(RequestOptions? options = null) =>
        GroupHelper.GetJoinRequestsAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.ApproveJoinRequestAsync(QQBot.GroupJoinRequest,QQBot.RequestOptions)" />
    public Task ApproveJoinRequestAsync(GroupJoinRequest request, RequestOptions? options = null) =>
        GroupHelper.ApproveJoinRequestAsync(this, Client, request.MemberId, request.Id, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.ApproveJoinRequestAsync(System.Guid,System.String,QQBot.RequestOptions)" />
    public Task ApproveJoinRequestAsync(Guid memberId, string? joinRequestId = null, RequestOptions? options = null) =>
        GroupHelper.ApproveJoinRequestAsync(this, Client, memberId, joinRequestId, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.DeclineJoinRequestAsync(QQBot.GroupJoinRequest,System.String,System.Boolean,QQBot.RequestOptions)" />
    public Task DeclineJoinRequestAsync(GroupJoinRequest request, string? reason = null,
        bool addToBlacklist = false, RequestOptions? options = null) =>
        GroupHelper.DeclineJoinRequestAsync(this, Client, request.MemberId, request.Id, reason, addToBlacklist, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.DeclineJoinRequestAsync(System.Guid,System.String,System.String,System.Boolean,QQBot.RequestOptions)" />
    public Task DeclineJoinRequestAsync(Guid memberId, string? joinRequestId = null,
        string? reason = null, bool addToBlacklist = false, RequestOptions? options = null) =>
        GroupHelper.DeclineJoinRequestAsync(this, Client, memberId, joinRequestId, reason, addToBlacklist, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.GetMuteSettingAsync(QQBot.RequestOptions)" />
    public Task<GroupMuteSetting> GetMuteSettingAsync(RequestOptions? options = null) =>
        GroupHelper.GetMuteSettingAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.MuteMemberAsync(System.Guid,System.DateTimeOffset,QQBot.RequestOptions)" />
    public Task MuteMemberAsync(Guid memberId, DateTimeOffset expiresAt, RequestOptions? options = null) =>
        GroupHelper.MuteMemberAsync(Id, Client, memberId, expiresAt, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.MuteMemberAsync(System.Guid,System.TimeSpan,QQBot.RequestOptions)" />
    public Task MuteMemberAsync(Guid memberId, TimeSpan duration, RequestOptions? options = null) =>
        GroupHelper.MuteMemberAsync(Id, Client, memberId, DateTimeOffset.UtcNow + duration, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.UnmuteMemberAsync(System.Guid,QQBot.RequestOptions)" />
    public Task UnmuteMemberAsync(Guid memberId, RequestOptions? options = null) =>
        GroupHelper.UnmuteMemberAsync(Id, Client, memberId, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.RemoveMembersAsync(System.Collections.Generic.IEnumerable{System.Guid},System.Boolean,QQBot.RequestOptions)" />
    public Task<GroupRemoveMembersResult> RemoveMembersAsync(IEnumerable<Guid> memberIds,
        bool addToBlacklist = false, RequestOptions? options = null) =>
        GroupHelper.RemoveMembersAsync(Id, Client, memberIds, addToBlacklist, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.RemoveMembersAsync(System.Collections.Generic.IEnumerable{QQBot.IGroupMember},System.Boolean,QQBot.RequestOptions)" />
    public Task<GroupRemoveMembersResult> RemoveMembersAsync(IEnumerable<IGroupMember> members,
        bool addToBlacklist = false, RequestOptions? options = null) =>
        RemoveMembersAsync(members.Select(x => x.Id), addToBlacklist, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.GetBlacklistAsync(QQBot.RequestOptions)" />
    public IAsyncEnumerable<IReadOnlyCollection<GroupBlacklistUser>> GetBlacklistAsync(RequestOptions? options = null) =>
        GroupHelper.GetBlacklistAsync(Id, Client, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.AddToBlacklistAsync(System.Collections.Generic.IEnumerable{System.Guid},QQBot.RequestOptions)" />
    public Task<IReadOnlyCollection<Guid>> AddToBlacklistAsync(IEnumerable<Guid> memberIds, RequestOptions? options = null) =>
        GroupHelper.AddToBlacklistAsync(Id, Client, memberIds, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.AddToBlacklistAsync(System.Collections.Generic.IEnumerable{QQBot.IGroupMember},QQBot.RequestOptions)" />
    public Task<IReadOnlyCollection<Guid>> AddToBlacklistAsync(IEnumerable<IGroupMember> members, RequestOptions? options = null) =>
        AddToBlacklistAsync(members.Select(x => x.Id), options);

    /// <inheritdoc cref="QQBot.IGroupChannel.RemoveFromBlacklistAsync(System.Collections.Generic.IEnumerable{System.Guid},QQBot.RequestOptions)" />
    public Task<IReadOnlyCollection<Guid>> RemoveFromBlacklistAsync(IEnumerable<Guid> memberIds, RequestOptions? options = null) =>
        GroupHelper.RemoveFromBlacklistAsync(Id, Client, memberIds, options);

    /// <inheritdoc cref="QQBot.IGroupChannel.RemoveFromBlacklistAsync(System.Collections.Generic.IEnumerable{QQBot.IGroupMember},QQBot.RequestOptions)" />
    public Task<IReadOnlyCollection<Guid>> RemoveFromBlacklistAsync(IEnumerable<IGroupMember> members, RequestOptions? options = null) =>
        RemoveFromBlacklistAsync(members.Select(x => x.Id), options);

    #endregion

    #region ISocketMessageChannel

    /// <inheritdoc />
    SocketMessage? ISocketMessageChannel.GetCachedMessage(string id) => null;

    #endregion

    #region IMessageChannel

    /// <inheritdoc />
    Task<IUserMessage> IMessageChannel.SendMessageAsync(string? content, IMarkdown? markdown,
        FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, IUserMessage? passiveSource, RequestOptions? options) =>
        SendMessageAsync(content, markdown, attachment, embed, ark, keyboard, messageReference, passiveSource, options);

    /// <inheritdoc />
    Task<IMessage?> IMessageChannel.GetMessageAsync(string id, CacheMode mode, RequestOptions? options)
        => Task.FromResult<IMessage?>(null);

    #endregion
}
