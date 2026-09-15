namespace QQBot;

/// <summary>
///     指定流式消息分片的输入状态。
/// </summary>
public enum StreamMessageInputState
{
    /// <summary>
    ///     消息仍在生成。
    /// </summary>
    Generating = 1,

    /// <summary>
    ///     消息生成结束。
    /// </summary>
    Completed = 10
}