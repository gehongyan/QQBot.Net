namespace QQBot;

/// <summary>
///     表示提醒类型。
/// </summary>
public enum RemindType
{
    /// <summary>
    ///     不提醒。
    /// </summary>
    None = 0,

    /// <summary>
    ///     开始时提醒。
    /// </summary>
    AtStart = 1,

    /// <summary>
    ///     开始前 5 分钟提醒。
    /// </summary>
    Before5Minutes = 2,

    /// <summary>
    ///     开始前 15 分钟提醒。
    /// </summary>
    Before15Minutes = 3,

    /// <summary>
    ///     开始前 30 分钟提醒。
    /// </summary>
    Before30Minutes = 4,

    /// <summary>
    ///     开始前 60 分钟提醒。
    /// </summary>
    Before60Minutes = 5
}
