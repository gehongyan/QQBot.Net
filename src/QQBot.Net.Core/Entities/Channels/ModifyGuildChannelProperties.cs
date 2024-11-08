namespace QQBot;

/// <summary>
///     提供用于修改 <see cref="QQBot.IGuildChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuildChannel.ModifyAsync(System.Action{QQBot.ModifyGuildChannelProperties},QQBot.RequestOptions)"/>
public class ModifyGuildChannelProperties
{
    /// <summary>
    ///     获取或设置要设置到此频道的新名称。
    /// </summary>
    /// <remarks>
    ///     如果此值为 <c>null</c>，则子频道的名称不会被修改。
    /// </remarks>
    public string? Name { get; set; }

    /// <summary>
    ///     获取或设置要设置到此频道的位置。
    /// </summary>
    /// <remarks>
    ///     更小的数值表示更靠近列表顶部的位置。设置为与同分组下的其他频道相同的值，将会使当前频道排列于与该频道相邻更靠近列表顶部的位置。
    /// </remarks>
    public virtual int? Position { get; set; }

    /// <summary>
    ///     获取或设置要设置到此频道的私有频道类型。
    /// </summary>
    /// <remarks>
    ///     如果此值为 <c>null</c>，则子频道的私有频道类型不会被修改。
    /// </remarks>
    public PrivateType? PrivateType { get; set; }

    /// <summary>
    ///     获取或设置要设置到此频道的发言权限。
    /// </summary>
    /// <remarks>
    ///     如果此值为 <c>null</c>，则子频道的发言权限不会被修改。
    /// </remarks>
    public SpeakPermission? SpeakPermission { get; set; }
}
