namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.IScheduleChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateScheduleChannelAsync(System.String,System.Action{QQBot.CreateScheduleChannelProperties},QQBot.RequestOptions)"/>
public class CreateScheduleChannelProperties : CreateNestedChannelProperties;