namespace QQBot;

/// <summary>
///     表示一个通用的 QQ Bot 客户端。
/// </summary>
public interface IQQBotClient : IDisposable
{
    #region General

    /// <summary>
    ///     获取当前连接的状态。
    /// </summary>
    ConnectionState ConnectionState { get; }

    /// <summary>
    ///     获取当前已登录的用户；如果没有用户登录，则为 <c>null</c>。
    /// </summary>
    ISelfUser? CurrentUser { get; }

    /// <summary>
    ///     获取已登录用户的令牌类型。
    /// </summary>
    TokenType TokenType { get; }

    /// <summary>
    ///     启动客户端与 QQ 之间的连接。
    /// </summary>
    /// <remarks>
    ///     当前方法会初始化客户端与 QQ 之间的连接。 <br />
    ///     <note type="important">
    ///         此方法会在调用后立即返回，因为它会在另一个线程上初始化连接。
    ///     </note>
    /// </remarks>
    /// <returns> 一个表示异步启动操作的任务。 </returns>
    Task StartAsync();

    /// <summary>
    ///     停止客户端与 QQ 之间的连接。
    /// </summary>
    /// <returns> 一个表示异步停止操作的任务。 </returns>
    Task StopAsync();

    /// <summary>
    ///     登录到 QQ API。
    /// </summary>
    /// <param name="appId"> 要使用的应用 ID。 </param>
    /// <param name="tokenType"> 要使用的令牌类型。 </param>
    /// <param name="token"> 要使用的令牌。 </param>
    /// <param name="validateToken"> 是否验证令牌。 </param>
    /// <returns> 一个表示异步登录操作的任务。 </returns>
    /// <remarks>
    ///     验证令牌的操作是通过 <see cref="QQBot.TokenUtils.ValidateToken(QQBot.TokenType,System.String)"/> 方法完成的。 <br />
    ///     此方法用于向当前客户端设置后续 API 请求的身份验证信息，获取并设置当前所登录用户的信息。
    /// </remarks>
    Task LoginAsync(int appId, TokenType tokenType, string token, bool validateToken = true);

    /// <summary>
    ///     从 QQ API 退出登录。
    /// </summary>
    /// <returns> 一个表示异步退出登录操作的任务。 </returns>
    /// <remarks>
    ///     此方法用于清除当前客户端的身份验证信息及所缓存的当前所登录的用户信息。
    /// </remarks>
    Task LogoutAsync();

    #endregion

    /// <summary>
    ///     Gets the gateway information related to the bot.
    /// </summary>
    /// <param name="options">The options to be used when sending the request.</param>
    /// <returns>
    ///     A task that represents the asynchronous get operation. The task result contains a <see cref="BotGateway"/>
    ///     that represents the gateway information related to the bot.
    /// </returns>
    Task<BotGateway> GetBotGatewayAsync(RequestOptions? options = null);
}
