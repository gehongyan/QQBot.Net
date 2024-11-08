namespace QQBot;

/// <summary>
///     提供用于修改 <see cref="QQBot.ICategoryChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.ICategoryChannel.ModifyAsync(System.Action{QQBot.ModifyCategoryChannelProperties},QQBot.RequestOptions)"/>
public class ModifyCategoryChannelProperties : ModifyGuildChannelProperties
{
    /// <inheritdoc />
    /// <remarks>
    ///     更小的数值表示更靠近列表顶部的位置。设置为与同分组下的其他频道相同的值，将会使当前频道排列于与该频道相邻更靠近列表顶部的位置。 <br />
    ///     分组频道的位置顺序号至少为 <c>2</c>。
    /// </remarks>
    public override int? Position { get; set; }
}
