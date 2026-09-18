using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一条订阅消息模板的授权状态变更结果。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SubscribeMessageTemplateResult
{
    /// <summary>
    ///     获取官方模板的标识符。
    /// </summary>
    public int TemplateId { get; }

    /// <summary>
    ///     获取自定义模板的标识符；若无则为 <see langword="null"/>。
    /// </summary>
    public string? CustomTemplateId { get; }

    /// <summary>
    ///     获取此模板的授权操作。
    /// </summary>
    public SubscribeMessageAuthorization Authorization { get; }

    /// <summary>
    ///     获取此订阅的标识符。
    /// </summary>
    public string SubscriptionId { get; }

    /// <summary>
    ///     获取此授权状态的更新时间。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; }

    internal SubscribeMessageTemplateResult(int templateId, string? customTemplateId,
        SubscribeMessageAuthorization authorization, string subscriptionId, DateTimeOffset updatedAt)
    {
        TemplateId = templateId;
        CustomTemplateId = customTemplateId;
        Authorization = authorization;
        SubscriptionId = subscriptionId;
        UpdatedAt = updatedAt;
    }

    private string DebuggerDisplay => $"{TemplateId} ({Authorization})";
}
