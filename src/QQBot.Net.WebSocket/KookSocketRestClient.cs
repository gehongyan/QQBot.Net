using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个用于网关客户端内的 REST 客户端。
/// </summary>
public class QQBotSocketRestClient : QQBotRestClient
{
    internal QQBotSocketRestClient(QQBotRestConfig config, API.QQBotRestApiClient api) : base(config, api)
    {
    }

    /// <inheritdoc cref="QQBot.Rest.BaseQQBotClient.LoginAsync(System.Int32,QQBot.TokenType,System.String,System.Boolean)" />
    /// <exception cref="NotSupportedException"> 网关客户端内的 REST 客户端无法进行登录或退出登录。 </exception>
    public new Task LoginAsync(int appId, TokenType tokenType, string token, bool validateToken = true) =>
        throw new NotSupportedException("The Socket REST wrapper cannot be used to log in or out.");

    internal override Task LoginInternalAsync(int appId, TokenType tokenType, string token, bool validateToken) =>
        throw new NotSupportedException("The Socket REST wrapper cannot be used to log in or out.");

    /// <inheritdoc cref="QQBot.Rest.BaseQQBotClient.LogoutAsync" />
    /// <exception cref="NotSupportedException"> 网关客户端内的 REST 客户端无法进行登录或退出登录。 </exception>
    public new Task LogoutAsync() =>
        throw new NotSupportedException("The Socket REST wrapper cannot be used to log in or out.");

    internal override Task LogoutInternalAsync() =>
        throw new NotSupportedException("The Socket REST wrapper cannot be used to log in or out.");
}
