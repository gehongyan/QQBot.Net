using System.Reflection;

namespace QQBot;

/// <summary>
///     定义 QQBot.Net 各种基础行为的配置项。
/// </summary>
public class QQBotConfig
{
    /// <summary>
    ///     获取 QQBot.Net 使用的 API 版本。
    /// </summary>
    public const int APIVersion = 2;

    /// <summary>
    ///     获取 QQBot.Net 使用的默认请求超时时间。
    /// </summary>
    /// <returns> 一个包含详细版本信息的字符串，包括构建号；当无法获取构建版本时为 <c>Unknown</c>。 </returns>
    public static string Version { get; } =
        typeof(QQBotConfig).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        ?? typeof(QQBotConfig).GetTypeInfo().Assembly.GetName().Version?.ToString(3)
        ?? "Unknown";

    /// <summary>
    ///     获取 QQBot.Net 在每个请求中使用的用户代理。
    /// </summary>
    public static string UserAgent { get; } = $"QQBot (https://github.com/gehongyan/QQBot.Net, v{Version})";

    /// <summary>
    ///     获取 QQ Bot API 请求的根 URL。
    /// </summary>
    public const string APIUrl = "https://api.sgroup.qq.com/";

    /// <summary>
    ///     获取 QQ Bot 沙箱环境 API 请求的根 URL。
    /// </summary>
    public const string SandboxAPIUrl = "https://sandbox.api.sgroup.qq.com/";

    /// <summary>
    ///     获取 QQ Bot 沙箱环境 API 请求的根 URL。
    /// </summary>
    public const string AccessTokenAPIUrl = "https://bots.qq.com/";

    /// <summary>
    ///     获取 QQ Bot 的 AccessToken 令牌刷新缓冲时长。
    /// </summary>
    public static readonly TimeSpan AccessTokenRefreshBuffer = TimeSpan.FromSeconds(60);

    /// <summary>
    ///     获取或设置 QQBot.Net 使用的接入环境。
    /// </summary>
    public AccessEnvironment AccessEnvironment { get; set; } = AccessEnvironment.Production;

    /// <summary>
    ///     获取请求超时的默认时间，以毫秒为单位。
    /// </summary>
    public const int DefaultRequestTimeout = 6000;

    /// <summary>
    ///     获取每请求获取子频道的最大数量。
    /// </summary>
    public const int MaxGuildsPerBatch = 100;

    /// <summary>
    ///     获取每请求获取子频道成员的最大数量。
    /// </summary>
    public const int MaxMembersPerBatch = 400;

    /// <summary>
    ///     获取每请求获取回应的用户的最大数量。
    /// </summary>
    public const int MaxReactionUsersPerBatch = 50;

    /// <summary>
    ///     获取或设置请求在出现错误时的默认行为。
    /// </summary>
    /// <seealso cref="QQBot.RequestOptions.RetryMode"/>
    public RetryMode DefaultRetryMode { get; set; } = RetryMode.AlwaysRetry;

    /// <summary>
    ///     获取或设置默认的速率限制回调委托。
    /// </summary>
    /// <remarks>
    ///     若同时设置了此属性与用于各个请求的 <see cref="QQBot.RequestOptions.RatelimitCallback"/>，则将优先使用
    ///     <see cref="QQBot.RequestOptions.RatelimitCallback"/>。
    /// </remarks>
    public Func<IRateLimitInfo, Task>? DefaultRatelimitCallback { get; set; }

    /// <summary>
    ///     获取或设置将发送到日志事件的最低日志严重性级别。
    /// </summary>
    public LogSeverity LogLevel { get; set; } = LogSeverity.Info;

    /// <summary>
    ///     获取或设置是否应打印初次启动时要打印的日志。
    /// </summary>
    /// <remarks>
    ///     如果设置为 <c>true</c>，则将在启动时打印库的当前版本，以及所使用的 API 版本。
    /// </remarks>
    internal bool DisplayInitialLog { get; set; } = true;

    /// <summary>
    ///     Returns the factor to reduce the heartbeat interval.
    /// </summary>
    /// <remarks>
    ///     If a heartbeat takes longer than the interval estimated by Discord, the connection will be closed.
    ///     This factor is used to reduce the interval and ensure that Discord will get the heartbeat within the estimated interval.
    /// </remarks>
    internal const double HeartbeatIntervalFactor = 0.9;

    /// <summary>
    ///     消息序号盐值类型。
    /// </summary>
    /// <remarks>
    ///     此属性决定了消息序号盐值的生成方式，进而影响消息序号的生成。 <br />
    ///     发送消息时，当此属性值不为 <see cref="QQBot.MessageSequenceGenerationParameters.None"/> 时，
    ///     则会将各个标志所对应的数据来源使用 <see cref="System.HashCode.Combine{T1,T2}(T1,T2)"/>
    ///     依次组合成一个整数作为消息序号盐值。 <br />
    ///     默认值为 <see cref="QQBot.MessageSequenceGenerationParameters.MessageParameters"/>，
    ///     表示每次发送消息时，都所要发送的消息的所有参数都会参与计算，所有参数均相同的消息将具有相同的消息序号盐值，相同的消息会被 QQ Bot
    ///     服务端忽略。设置了 <see cref="QQBot.MessageSequenceGenerationParameters.AutoIncrement"/>、
    ///     <see cref="QQBot.MessageSequenceGenerationParameters.Timestamp"/> 或
    ///     <see cref="QQBot.MessageSequenceGenerationParameters.Random"/>
    ///     时，会使每条消息的消息序号盐值都包含一个不同的值，从而使每条消息的消息序号都不同，即使消息内容完全相同也不会被 QQ Bot 服务端忽略。
    /// </remarks>
    public MessageSequenceGenerationParameters MessageSequenceGenerationParameters { get; set; } =
        MessageSequenceGenerationParameters.MessageParameters;
}
