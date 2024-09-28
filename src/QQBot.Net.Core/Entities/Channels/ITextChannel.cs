namespace QQBot;

/// <summary>
///     表示一个文字子频道。
/// </summary>
public interface ITextChannel : INestedChannel
{
    /// <summary>
    ///     获取此子频道的二级分类。
    /// </summary>
    /// <remarks>
    ///     仅当 <see cref="Type"/> 为 <see cref="ChannelType.Text"/> 时此属性才有值。
    /// </remarks>
    ChannelSubType? SubType { get; }
}
