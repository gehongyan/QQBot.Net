namespace QQBot;

/// <summary>
///     表示一个应用子频道。
/// </summary>
public interface IApplicationChannel : INestedChannel
{
    /// <summary>
    ///     获取此子频道的应用类型。
    /// </summary>
    ChannelApplication? ApplicationType { get; }
}
