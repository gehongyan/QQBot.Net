namespace QQBot;

/// <summary>
///     配置上传任务的最终合并行为。
/// </summary>
internal class MediaUploadCompletionOptions
{
    /// <summary>
    ///     获取或设置是否在合并成功后直接发送该富媒体消息。
    /// </summary>
    /// <remarks> 直接发送会消耗主动消息频次；默认值为 <see langword="false"/>。 </remarks>
    public bool SendMessage { get; set; }
}