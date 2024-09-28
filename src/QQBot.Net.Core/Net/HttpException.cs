using System.Collections.Immutable;
using System.Net;

namespace QQBot.Net;

/// <summary>
///     当处理 QQBot HTTP 请求时发生错误时引发的异常。
/// </summary>
public class HttpException : Exception
{
    /// <summary>
    ///     获取 QQBot 返回的 HTTP 状态码。
    /// </summary>
    public HttpStatusCode HttpCode { get; }

    /// <summary>
    ///     获取由 QQBot 返回的 JSON 负载中的错误代码；也有可能是表示操作成功的代码；
    ///     如果无法从响应中解析出错误代码，则为 <see langword="null"/>。
    /// </summary>
    public QQBotErrorCode? QQBotCode { get; }

    /// <summary>
    ///     获取异常的原因；也有可能是表示操作成功的消息；如果无法从响应中解析出原因，则为 <see langword="null"/>。
    /// </summary>
    public string? Reason { get; }

    /// <summary>
    ///     获取错误代码。
    /// </summary>
    public int? ErrorCode { get; }

    /// <summary>
    ///     获取跟踪 ID。
    /// </summary>
    public string? TraceId { get; }

    /// <summary>
    ///     获取用于发送请求的请求对象。
    /// </summary>
    public IRequest Request { get; }

    /// <summary>
    ///     初始化一个 <see cref="HttpException"/> 类的新实例。
    /// </summary>
    /// <param name="httpCode"> 返回的 HTTP 状态码。 </param>
    /// <param name="request"> 引发异常前发送的请求。 </param>
    /// <param name="qqBotCode"> 由 QQBot 返回的 JSON 负载中解析出的状态码。 </param>
    /// <param name="reason"> 引发异常的原因。 </param>
    /// <param name="errorCode"> 错误码。 </param>
    /// <param name="traceId"> 跟踪 ID。 </param>
    public HttpException(HttpStatusCode httpCode, IRequest request,
        QQBotErrorCode? qqBotCode = null, string? reason = null, int? errorCode = null, string? traceId = null)
        : base(CreateMessage(httpCode, (int?)qqBotCode, reason))
    {
        HttpCode = httpCode;
        Request = request;
        QQBotCode = qqBotCode;
        Reason = reason;
        ErrorCode = errorCode;
        TraceId = traceId;
    }

    private static string CreateMessage(HttpStatusCode httpCode, int? qqBotCode = null, string? reason = null)
    {
        int closeCode = qqBotCode.HasValue && qqBotCode != 0
            ? qqBotCode.Value
            : (int) httpCode;
        return reason != null
            ? $"The server responded with error {closeCode}: {reason}"
            : $"The server responded with error {closeCode}: {httpCode}";
    }
}
