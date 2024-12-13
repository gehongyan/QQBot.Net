namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的日程。
/// </summary>
public class RestGuildSchedule : RestEntity<ulong>, IGuildSchedule
{
    /// <inheritdoc />
    public IScheduleChannel Channel { get; }

    /// <inheritdoc />
    public string Name { get; private set; }

    /// <inheritdoc />
    public string? Description { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset StartTime { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset EndTime { get; private set; }

    /// <inheritdoc />
    public IGuildMember? Creator { get; private set; }

    /// <inheritdoc />
    public ulong? JumpChannelId { get; private set; }

    /// <inheritdoc />
    public RemindType RemindType { get; private set; }

    internal RestGuildSchedule(BaseQQBotClient client, IScheduleChannel channel, ulong id,
        string name, string? description, DateTimeOffset startTime, DateTimeOffset endTime,
        IGuildMember? creator, ulong? jumpChannelId, RemindType remindType)
        : base(client, id)
    {
        Channel = channel;
        Name = name;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        Creator = creator;
        JumpChannelId = jumpChannelId;
        RemindType = remindType;
    }

    internal static RestGuildSchedule Create(BaseQQBotClient client,
        IScheduleChannel channel, API.Schedule model, IGuildMember? creator) =>
        new(client, channel, model.Id,
            model.Name, model.Description, model.StartTimestamp, model.EndTimestamp,
            creator, model.JumpChannelId != 0 ? model.JumpChannelId : null, model.RemindType);

    internal void Update(API.Schedule model)
    {
        Name = model.Name;
        Description = model.Description;
        StartTime = model.StartTimestamp;
        EndTime = model.EndTimestamp;
        if (model.Creator.User is { } userModel && Creator is RestGuildMember creator)
            creator.Update(userModel, model.Creator);
        JumpChannelId = model.JumpChannelId != 0 ? model.JumpChannelId : null;
        RemindType = model.RemindType;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(RequestOptions? options = null)
    {
        API.Schedule model = await ChannelHelper.GetScheduleAsync(Channel, Client, Id, options).ConfigureAwait(false);
        Update(model);
    }

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyGuildScheduleProperties> func, RequestOptions? options = null)
    {
        API.Schedule model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(model);
    }

    /// <inheritdoc />
    public Task DeleteAsync(RequestOptions? options = null) =>
        ChannelHelper.DeleteAsync(this, Client, options);
}
