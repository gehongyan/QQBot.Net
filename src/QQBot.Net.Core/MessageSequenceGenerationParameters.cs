namespace QQBot;

/// <summary>
///     消息序号生成算法所包含的参数。
/// </summary>
[Flags]
public enum MessageSequenceGenerationParameters
{
    /// <summary>
    ///     不使用参数。
    /// </summary>
    /// <remarks>
    ///     仅使用此值会使 QQBot.Net 在发送消息时为每条消息的消息顺序号都指定为固定值。
    /// </remarks>
    None = 0,

    /// <summary>
    ///     包含一个自增序列。
    /// </summary>
    AutoIncrement = 1 << 0,

    /// <summary>
    ///     包含当前时间戳。
    /// </summary>
    Timestamp = 1 << 1,

    /// <summary>
    ///     包含一个随机数。
    /// </summary>
    Random = 1 << 2,

    /// <summary>
    ///     包含消息内容参数。
    /// </summary>
    Content = 1 << 3,

    /// <summary>
    ///     包含 Markdown 参数。
    /// </summary>
    Markdown = 1 << 4,

    /// <summary>
    ///     包含文件附件参数。
    /// </summary>
    FileAttachment = 1 << 5,

    /// <summary>
    ///     包含 Embed 参数。
    /// </summary>
    Embed = 1 << 6,

    /// <summary>
    ///     包含 Ark 参数。
    /// </summary>
    Ark = 1 << 7,

    /// <summary>
    ///     包含 Keyboard 参数。
    /// </summary>
    Keyboard = 1 << 8,

    /// <summary>
    ///     包含消息引用参数。
    /// </summary>
    MessageReference = 1 << 9,

    /// <summary>
    ///     包含被动消息所关联的消息。
    /// </summary>
    PassiveSource = 1 << 10,

    /// <summary>
    ///     包含所有消息参数。
    /// </summary>
    MessageParameters = Content | Markdown | FileAttachment | Embed | Ark | Keyboard | MessageReference | PassiveSource,

    /// <summary>
    ///     包含所有可用参数。
    /// </summary>
    All = AutoIncrement | Timestamp | Random | MessageParameters
}
