namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.ILiveStreamChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateLiveStreamChannelAsync(System.String,System.Action{QQBot.CreateLiveStreamChannelProperties},QQBot.RequestOptions)"/>
public class CreateLiveStreamChannelProperties : CreateGuildChannelProperties
{
    /// <summary>
    ///     获取或设置要设置到此频道的私有频道类型。
    /// </summary>
    public PrivateType? PrivateType { get; set; }

    /// <summary>
    ///     获取或设置要设置到此频道的发言权限。
    /// </summary>
    public SpeakPermission? SpeakPermission { get; set; }
}
