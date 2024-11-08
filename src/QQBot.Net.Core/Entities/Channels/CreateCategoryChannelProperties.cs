namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.ICategoryChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateCategoryChannelAsync(System.String,System.Action{QQBot.CreateCategoryChannelProperties},QQBot.RequestOptions)"/>
public class CreateCategoryChannelProperties : CreateGuildChannelProperties
{
    /// <summary>
    ///     获取或设置要设置到此频道的位置。
    /// </summary>
    /// <remarks>
    ///     更小的数值表示更靠近列表顶部的位置。设置为与同分组下的其他频道相同的值，将会使当前频道排列于与该频道相邻更靠近列表顶部的位置。
    ///     分组频道的位置顺序号至少为 <c>2</c>。
    /// </remarks>
    public int Position { get; set; } = 2;
}
