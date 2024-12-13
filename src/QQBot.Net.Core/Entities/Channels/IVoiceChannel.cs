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

    /// <summary>
    ///     获取此子频道内在线用户的数量。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步获取操作的任务。任务的结果包含此子频道内在线用户的数量；如果无法获取，则返回 <c>null</c>。 </returns>
    Task<int> CountOnlineUsersAsync(RequestOptions? options = null);

    /// <summary>
    ///     语音连接到此子频道。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task JoinAsync(RequestOptions? options = null);

    /// <summary>
    ///     语音断开连接。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task LeaveAsync(RequestOptions? options = null);

    /// <summary>
    ///     开始播放指定 URL 的音频。
    /// </summary>
    /// <param name="url"> 要播放的音频的 URL。 </param>
    /// <param name="displayText"> 状态文本。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task PlayAsync(Uri url, string displayText, RequestOptions? options = null);

    /// <summary>
    ///     开始播放指定 URL 的音频。
    /// </summary>
    /// <param name="url"> 要播放的音频的 URL。 </param>
    /// <param name="displayText"> 状态文本。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task PlayAsync(string url, string displayText, RequestOptions? options = null);

    /// <summary>
    ///     暂停当前播放的音频。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task PauseAsync(RequestOptions? options = null);

    /// <summary>
    ///     暂停当前播放的音频。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task ResumeAsync(RequestOptions? options = null);

    /// <summary>
    ///     停止当前播放的音频。
    /// </summary>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task StopAsync(RequestOptions? options = null);
}
