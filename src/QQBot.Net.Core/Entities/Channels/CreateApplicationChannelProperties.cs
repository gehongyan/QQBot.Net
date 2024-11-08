namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.IApplicationChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateApplicationChannelAsync(System.String,System.Action{QQBot.CreateApplicationChannelProperties},QQBot.RequestOptions)"/>
public class CreateApplicationChannelProperties : CreateNestedChannelProperties
{
    /// <summary>
    ///     获取或设置要设置到此频道的应用频道类型。
    /// </summary>
    public ChannelApplication? ApplicationType { get; set; }
}