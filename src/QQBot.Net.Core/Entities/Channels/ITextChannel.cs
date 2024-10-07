namespace QQBot;

/// <summary>
///     表示一个文字子频道。
/// </summary>
public interface ITextChannel : IMessageChannel, INestedChannel, IMentionable
{
    /// <summary>
    ///     获取此子频道的二级分类。
    /// </summary>
    ChannelSubType? SubType { get; }
}
