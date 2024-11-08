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

    /// <summary>
    ///     修改此应用子频道。
    /// </summary>
    /// <param name="func"> 一个包含修改应用子频道属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示应用子频道属性修改操作的异步任务。 </returns>
    Task ModifyAsync(Action<ModifyApplicationChannelProperties> func, RequestOptions? options = null);
}
