namespace QQBot;

internal static class UrlValidation
{
    /// <summary>
    ///     检查 URL 是否有效。
    /// </summary>
    /// <param name="url"> 要校验的 URL。 </param>
    /// <exception cref="UriFormatException"> URL 不能为空。 </exception>
    /// <exception cref="UriFormatException"> URL 必须包含协议（HTTP 或 HTTPS）。 </exception>
    /// <returns> 如果 URL 有效，则为 <c>true</c>，否则为 <c>false</c>。 </returns>
    /// <remarks>
    ///     当前方法仅检查 URL 是否非空，且指定了 HTTP 或 HTTPS 协议。
    /// </remarks>
    public static void Validate(string url)
    {
        if (string.IsNullOrEmpty(url))
            throw new UriFormatException("The URL cannot be null or empty.");

        if (!(url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            throw new UriFormatException($"The url {url} must include a protocol (either HTTP or HTTPS)");
    }
}
