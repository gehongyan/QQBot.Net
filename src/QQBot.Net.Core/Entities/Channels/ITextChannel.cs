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

    /// <summary>
    ///     修改此频道有关文字聊天能力的属性。
    /// </summary>
    /// <param name="func"> 一个包含修改频道有关文字聊天能力的属性的委托。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步修改操作的任务。 </returns>
    Task ModifyAsync(Action<ModifyTextChannelProperties> func, RequestOptions? options = null);

    /// <summary>
    ///     请求应用程序权限。
    /// </summary>
    /// <param name="title"> 接口权限链接中的接口权限描述信息。 </param>
    /// <param name="permission"> 要请求的权限。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RequestApplicationPermissionAsync(string title, ApplicationPermission permission,
        RequestOptions? options = null);

    /// <summary>
    ///     请求应用程序权限。
    /// </summary>
    /// <param name="title"> 接口权限链接中的接口权限描述信息。 </param>
    /// <param name="description"> 接口权限链接中的机器人可使用功能的描述信息。 </param>
    /// <param name="method"> 要请求的权限的请求方法。 </param>
    /// <param name="path"> 要请求的权限的路径。 </param>
    /// <param name="options"> 发送请求时要使用的选项。 </param>
    /// <returns> 一个表示异步操作的任务。 </returns>
    Task RequestApplicationPermissionAsync(string title, string description,
        HttpMethod method, string path, RequestOptions? options = null);
}
