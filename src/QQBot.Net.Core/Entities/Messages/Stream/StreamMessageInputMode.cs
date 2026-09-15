namespace QQBot;

/// <summary>
///     指定流式消息分片的输入模式。
/// </summary>
public enum StreamMessageInputMode
{
    /// <summary>
    ///     将分片内容追加到待发送内容。
    /// </summary>
    Append,

    /// <summary>
    ///     使用分片内容替换当前完整内容。
    /// </summary>
    /// <remarks>
    ///     内容必须保留 QQ 已下发内容的前缀；否则平台会拒绝该分片。
    /// </remarks>
    Replace
}