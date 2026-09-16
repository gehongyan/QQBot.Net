using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示菜单开关项的配置。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MenuSwitch
{
    /// <summary>
    ///     获取开关的唯一标识。
    /// </summary>
    /// <remarks>
    ///     用户切换开关状态后会发送一条消息，消息内容会携带此标识。例如标识为 <c>search</c> 时，用户打开开关后消息的
    ///     扩展字段中会携带 <c>search=1</c>，关闭后则不携带。
    /// </remarks>
    public string SwitchId { get; }

    /// <summary>
    ///     获取开关的初始状态；<see langword="true"/> 表示默认打开，<see langword="false"/> 表示默认关闭。
    /// </summary>
    public bool IsDefaultOn { get; }

    internal MenuSwitch(string switchId, bool isDefaultOn)
    {
        SwitchId = switchId;
        IsDefaultOn = isDefaultOn;
    }

    private string DebuggerDisplay => $"{SwitchId} ({(IsDefaultOn ? "On" : "Off")})";
}
