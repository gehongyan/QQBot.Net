namespace QQBot;

/// <summary>
///     表示指令面板的生效场景。
/// </summary>
public enum CommandPanelScope
{
    /// <summary>
    ///     QQ 单聊。
    /// </summary>
    C2C,

    /// <summary>
    ///     QQ 群聊。
    /// </summary>
    Group,

    /// <summary>
    ///     文字子频道。
    /// </summary>
    Channel,

    /// <summary>
    ///     频道私信。
    /// </summary>
    DM
}
