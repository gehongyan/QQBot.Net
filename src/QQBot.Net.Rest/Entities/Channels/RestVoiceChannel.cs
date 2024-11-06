using System.Diagnostics;
using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示频道中的一个基于 REST 的具有语音聊天能力的子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestVoiceChannel : RestGuildChannel, IVoiceChannel
{
    /// <inheritdoc />
    public ulong? CategoryId { get; private set; }

    /// <inheritdoc />
    public ChannelPrivateType? PrivateType { get; private set; }

    /// <inheritdoc />
    public SpeakPermission? SpeakPermission { get; private set; }

    /// <inheritdoc />
    public ChannelPermission? Permission { get; private set; }

    internal RestVoiceChannel(BaseQQBotClient client, ulong id, IGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Voice;
    }

    internal static new RestVoiceChannel Create(BaseQQBotClient client, IGuild guild, Channel model)
    {
        RestVoiceChannel entity = new(client, model.Id, guild);
        entity.Update(model);
        return entity;
    }

    internal override void Update(Channel model)
    {
        base.Update(model);
        CategoryId = model.ParentId;
        PrivateType = model.PrivateType;
        SpeakPermission = model.SpeakPermission;
        Permission = model.Permissions is not null ? Enum.Parse<ChannelPermission>(model.Permissions) : null; // TODO
    }

    private string DebuggerDisplay => $"{Name} ({Id}, Voice)";
}
