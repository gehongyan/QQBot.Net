namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的日程。
/// </summary>
public class RestGuildSchedule : RestEntity<ulong>, IGuildSchedule
{
    /// <inheritdoc />
    public IScheduleChannel Channel { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string? Description { get; }

    /// <inheritdoc />
    public DateTimeOffset StartTime { get; }

    /// <inheritdoc />
    public DateTimeOffset EndTime { get; }

    /// <inheritdoc />
    public IGuildMember? Creator { get; }

    /// <inheritdoc />
    public ulong? JumpChannelId { get; }

    /// <inheritdoc />
    public RemindType? RemindType { get; }

    internal RestGuildSchedule(BaseQQBotClient client, IScheduleChannel channel, ulong id,
        string name, string? description, DateTimeOffset startTime, DateTimeOffset endTime,
        IGuildMember? creator, ulong? jumpChannelId, RemindType? remindType)
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
}
