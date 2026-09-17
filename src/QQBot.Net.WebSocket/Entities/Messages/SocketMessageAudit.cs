using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的消息审核结果。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketMessageAudit
{
    /// <summary>
    ///     获取此审核结果所属的频道。
    /// </summary>
    public SocketGuild Guild { get; }

    /// <summary>
    ///     获取此审核结果所属的子频道。
    /// </summary>
    public SocketTextChannel Channel { get; }

    /// <summary>
    ///     获取此次审核的唯一标识符。
    /// </summary>
    public string AuditId { get; }

    /// <summary>
    ///     获取审核通过后消息的 ID。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="Result"/> 为 <see cref="QQBot.MessageAuditResult.Passed"/> 时可能有值；审核未通过时消息未发出，此属性为 <see langword="null"/>。
    /// </remarks>
    public string? MessageId { get; }

    /// <summary>
    ///     获取此次审核完成的时间。
    /// </summary>
    public DateTimeOffset AuditTime { get; }

    /// <summary>
    ///     获取此次审核任务创建的时间。
    /// </summary>
    public DateTimeOffset CreateTime { get; }

    /// <summary>
    ///     获取此次审核的结果。
    /// </summary>
    public MessageAuditResult Result { get; }

    private SocketMessageAudit(SocketTextChannel channel, API.Gateway.MessageAuditEvent model, MessageAuditResult result)
    {
        Guild = channel.Guild;
        Channel = channel;
        AuditId = model.AuditId;
        MessageId = model.MessageId;
        AuditTime = model.AuditTime;
        CreateTime = model.CreateTime;
        Result = result;
    }

    internal static SocketMessageAudit Create(SocketTextChannel channel,
        API.Gateway.MessageAuditEvent model, MessageAuditResult result) =>
        new(channel, model, result);

    private string DebuggerDisplay => $"{AuditId} ({Result})";
}
