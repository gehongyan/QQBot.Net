namespace QQBot;

/// <summary>
///     提供用于创建或修改指令面板的可选属性。
/// </summary>
public class CommandPanelProperties
{
    /// <summary>
    ///     获取或设置面板备注，最多 255 个字符，用于开发者标记面板用途，不对用户展示。
    /// </summary>
    public string? Remark { get; set; }
}
