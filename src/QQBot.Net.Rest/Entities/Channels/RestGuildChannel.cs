using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的频道内的子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestGuildChannel : RestChannel, IGuildChannel
{
    /// <inheritdoc cref="QQBot.IGuildChannel.Id" />
    public new ulong Id { get; }

    /// <inheritdoc cref="QQBot.IGuildChannel.Guild" />
    public IGuild Guild { get; }

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public ChannelType Type { get; internal set; }

    /// <inheritdoc />
    public int? Position { get; private set; }

    /// <inheritdoc />
    public ulong? CreatorId { get; private set; }

    internal RestGuildChannel(BaseQQBotClient client, ulong id, IGuild guild)
        : base(client, id.ToIdString())
    {
        Id = id;
        Guild = guild;
        Name = string.Empty;
        Type = ChannelType.Unspecified;
    }

    internal static RestGuildChannel Create(BaseQQBotClient client, IGuild guild, Model model) =>
        model.Type switch
        {
            ChannelType.Category => RestCategoryChannel.Create(client, guild, model),
            ChannelType.Text => RestTextChannel.Create(client, guild, model),
            ChannelType.Voice => RestVoiceChannel.Create(client, guild, model),
            ChannelType.LiveStream => RestLiveStreamChannel.Create(client, guild, model),
            ChannelType.Application => RestApplicationChannel.Create(client, guild, model),
            ChannelType.Forum => RestForumChannel.Create(client, guild, model),
            ChannelType.Schedule => RestScheduleChannel.Create(client, guild, model),
            _ => new RestGuildChannel(client, model.Id, guild)
        };

    internal virtual void Update(Model model)
    {
        Name = model.Name;
        Position = model.Position;
        CreatorId = model.OwnerId;
    }

    /// <inheritdoc />
    public virtual Task UpdateAsync(RequestOptions? options = null) =>
        ChannelHelper.UpdateAsync(this, options);

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyGuildChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(model);
    }

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) => ChannelHelper.DeleteAsync(this, Client, options);

    /// <inheritdoc cref="QQBot.Rest.RestGuildChannel.Name" />
    public override string ToString() => Name;

    private string DebuggerDisplay => $"{Name} ({Id}, Guild)";

    #region IChannel

    /// <inheritdoc />
    Task<IUser?> IChannel.GetUserAsync(string id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(null);

    #endregion

    #region IGuildChannel

    /// <inheritdoc />
    IGuild IGuildChannel.Guild => Guild;

    /// <inheritdoc />
    ulong IGuildChannel.GuildId => Guild.Id;

    /// <inheritdoc />
    Task<IGuildUser?> IGuildChannel.GetUserAsync(ulong id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IGuildUser?>(null);

    #endregion
}
