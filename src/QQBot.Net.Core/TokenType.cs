namespace QQBot;

/// <summary>
///     表示客户端所使用的令牌类型。
/// </summary>
public enum TokenType
{
    /// <summary>
    ///     OAuth2 令牌类型。
    /// </summary>
    BearerToken,

    /// <summary>
    ///     机器人令牌类型。
    /// </summary>
    BotToken,

    /// <summary>
    ///     机器人密钥类型。
    /// </summary>
    AppSecret
}
