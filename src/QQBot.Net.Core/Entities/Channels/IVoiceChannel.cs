namespace QQBot;

/// <summary>
///     表示一个语音子频道。
/// </summary>
public interface IVoiceChannel : INestedChannel
{
    /// <summary>
    ///     修改此频道有关语音聊天能力的属性。
    /// </summary>
    /// <param name="func"> 一个包含修改频道有关语音聊天能力的属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。 </returns>
    Task ModifyAsync(Action<ModifyVoiceChannelProperties> func, RequestOptions? options = null);
}
