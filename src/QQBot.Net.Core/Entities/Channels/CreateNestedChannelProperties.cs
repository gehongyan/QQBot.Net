namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.INestedChannel" /> 的属性。
/// </summary>
public class CreateNestedChannelProperties : CreateGuildChannelProperties
{
    // /// <summary>
    // ///     获取或设置要设置到此频道的所属分组频道的 ID。
    // /// </summary>
    // /// <remarks>
    // ///     将此值设置为某分组频道的 ID 可以使新建频道位于该分组频道下；将此值设置为 <c>null</c>
    // ///     可以使新建频道位于服务器所有分组频道的上方，即不属于任何分组频道。
    // /// </remarks>
    // public ulong? CategoryId { get; set; }

    /// <summary>
    ///     获取或设置要设置到此频道的私有频道类型。
    /// </summary>
    public PrivateType? PrivateType { get; set; }

    /// <summary>
    ///     获取或设置要设置到此频道的发言权限。
    /// </summary>
    public SpeakPermission? SpeakPermission { get; set; }
}