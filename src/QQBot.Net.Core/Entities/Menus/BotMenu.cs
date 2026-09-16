using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示机器人的全局自定义菜单。
/// </summary>
/// <remarks>
///     全局自定义菜单仅在 QQ 单聊（C2C）场景生效，设置后对所有用户生效，不支持按用户维度区分。
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class BotMenu
{
    /// <summary>
    ///     获取当前菜单的版本号。
    /// </summary>
    public int Version { get; }

    /// <summary>
    ///     获取菜单项列表，最多 10 个，按列表顺序从左到右展示。
    /// </summary>
    public IReadOnlyCollection<MenuItem> Items { get; }

    internal BotMenu(int version, IReadOnlyCollection<MenuItem> items)
    {
        Version = version;
        Items = items;
    }

    private string DebuggerDisplay => $"{Items.Count} Item{(Items.Count == 1 ? string.Empty : "s")} (v{Version})";
}
