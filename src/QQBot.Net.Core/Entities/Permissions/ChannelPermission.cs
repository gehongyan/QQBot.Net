namespace QQBot;

/// <summary>
///     表示可以为角色或用户设置的子频道级别的权限。
/// </summary>
[Flags]
public enum ChannelPermission : uint
{
    /// <inheritdoc cref="QQBot.GuildPermission.CreateInvites" />
    CreateInvites = 1 << 3,

    /// <inheritdoc cref="QQBot.GuildPermission.ManageChannels" />
    ManageChannels = 1 << 5,

    /// <inheritdoc cref="QQBot.GuildPermission.ManageRoles" />
    ManageRoles = 1 << 10,

    /// <inheritdoc cref="QQBot.GuildPermission.ViewChannel" />
    ViewChannel = 1 << 11,

    /// <inheritdoc cref="QQBot.GuildPermission.SendMessages" />
    SendMessages = 1 << 12,

    /// <inheritdoc cref="QQBot.GuildPermission.ManageMessages" />
    ManageMessages = 1 << 13,

    /// <inheritdoc cref="QQBot.GuildPermission.AttachFiles" />
    AttachFiles = 1 << 14,

    /// <inheritdoc cref="QQBot.GuildPermission.Connect" />
    Connect = 1 << 15,

    /// <inheritdoc cref="QQBot.GuildPermission.ManageVoice" />
    ManageVoice = 1 << 16,

    /// <inheritdoc cref="QQBot.GuildPermission.MentionEveryone" />
    MentionEveryone = 1 << 17,

    /// <inheritdoc cref="QQBot.GuildPermission.AddReactions" />
    AddReactions = 1 << 18,

    /// <inheritdoc cref="QQBot.GuildPermission.PassiveConnect" />
    PassiveConnect = 1 << 20,

    /// <inheritdoc cref="QQBot.GuildPermission.UseVoiceActivity" />
    UseVoiceActivity = 1 << 22,

    /// <inheritdoc cref="QQBot.GuildPermission.Speak" />
    Speak = 1 << 23,

    /// <inheritdoc cref="QQBot.GuildPermission.DeafenMembers" />
    DeafenMembers = 1 << 24,

    /// <inheritdoc cref="QQBot.GuildPermission.MuteMembers" />
    MuteMembers = 1 << 25,

    /// <inheritdoc cref="QQBot.GuildPermission.PlaySoundtrack" />
    PlaySoundtrack = 1 << 27,

    /// <inheritdoc cref="QQBot.GuildPermission.ShareScreen" />
    ShareScreen = 1 << 28
}
