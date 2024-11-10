namespace QQBot;

/// <summary>
///     提供一组用于处理 QQBot 登录令牌的辅助方法。
/// </summary>
public static class TokenUtils
{
    /// <summary>
    ///     QQ Bot 令牌的标准长度。
    /// </summary>
    /// <remarks>
    ///     此值是通过与 QQ Bot 文档和现有令牌的示例进行比较确定的。
    /// </remarks>
    internal const int StandardBotTokenLength = 32;

    /// <summary>
    ///     校验 Bot 令牌的有效性。
    /// </summary>
    /// <param name="token"> 要校验的 Bot 令牌。 </param>
    /// <returns> 如果校验成功，则返回 <c>true</c>；否则返回 <c>false</c>。 </returns>
    internal static bool CheckBotTokenOrAppSecretValidity(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;
#if NET7_0_OR_GREATER
        return token.All(x => char.IsAsciiLetterOrDigit(x));
#else
        return token.All(x => x is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z');
#endif
    }

    /// <summary>
    ///     令牌中不允许的所有字符。
    /// </summary>
    internal static readonly char[] IllegalTokenCharacters = [' ', '\t', '\r', '\n'];

    /// <summary>
    ///     检查给定的令牌是否包含会导致登录失败的空格或换行符。
    /// </summary>
    /// <param name="token"> 要检查的令牌。 </param>
    /// <returns> 如果令牌包含空格或换行符，则返回 <c>true</c>；否则返回 <c>false</c>。 </returns>
    internal static bool CheckContainsIllegalCharacters(string token) =>
        token.IndexOfAny(IllegalTokenCharacters) != -1;

    /// <summary>
    ///     检查指定类型的令牌的有效性。
    /// </summary>
    /// <param name="tokenType"> 令牌的类型。 </param>
    /// <param name="token"> 要校验的令牌。 </param>
    /// <exception cref="ArgumentNullException"> 当提供的令牌值为 <c>null</c>、空字符串或仅包含空白字符时引发异常。 </exception>
    /// <exception cref="ArgumentException"> 当提供的令牌类型或令牌值无效时引发异常。 </exception>
    public static void ValidateToken(TokenType tokenType, string token)
    {
        // A Null or WhiteSpace token of any type is invalid.
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentNullException(nameof(token), "A token cannot be null, empty, or contain only whitespace.");

        // ensure that there are no whitespace or newline characters
        if (CheckContainsIllegalCharacters(token))
            throw new ArgumentException("The token contains a whitespace or newline character. Ensure that the token has been properly trimmed.",
                nameof(token));

        switch (tokenType)
        {
            case TokenType.BearerToken:
                // no validation is performed on Bearer tokens
                break;
            case TokenType.BotToken or TokenType.AppSecret:
                // bot tokens are assumed to be at least 32 characters in length
                // this value was determined by referencing examples in the QQBot documentation, and by comparing with
                // pre-existing tokens
                if (token.Length != StandardBotTokenLength)
                    throw new ArgumentException($"A Bot token must be {StandardBotTokenLength} characters in length.", nameof(token));

                // check the validity of the bot token by decoding the ulong userid from the jwt
                if (!CheckBotTokenOrAppSecretValidity(token))
                    throw new ArgumentException("The Bot token was invalid.",
                        nameof(token));

                break;
            default:
                // All unrecognized TokenTypes (including User tokens) are considered to be invalid.
                throw new ArgumentException("Unrecognized TokenType.", nameof(token));
        }
    }
}
