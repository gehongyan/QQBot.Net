using System.Diagnostics;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的音频动作。
/// </summary>
/// <remarks>
///     音频动作发生在语音子频道内，涵盖音频开始播放、播放结束、上麦与下麦。
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketAudioAction
{
    /// <summary>
    ///     获取此音频动作所属的频道。
    /// </summary>
    public SocketGuild Guild { get; }

    /// <summary>
    ///     获取此音频动作所在的语音子频道。
    /// </summary>
    public SocketVoiceChannel Channel { get; }

    /// <summary>
    ///     获取音频的 URL。
    /// </summary>
    /// <remarks>
    ///     仅音频播放相关动作可能包含此信息；上麦与下麦动作通常为 <see langword="null"/>。
    /// </remarks>
    public string? AudioUrl { get; }

    /// <summary>
    ///     获取音频的文本描述（如歌曲名称）。
    /// </summary>
    /// <remarks>
    ///     仅音频播放相关动作可能包含此信息；上麦与下麦动作通常为 <see langword="null"/>。
    /// </remarks>
    public string? Text { get; }

    private SocketAudioAction(SocketVoiceChannel channel, string? audioUrl, string? text)
    {
        Guild = channel.Guild;
        Channel = channel;
        AudioUrl = audioUrl;
        Text = text;
    }

    internal static SocketAudioAction Create(SocketVoiceChannel channel, API.Gateway.AudioActionEvent model) =>
        new(channel, model.AudioUrl, model.Text);

    private string DebuggerDisplay => $"{Channel.Name} ({Text ?? AudioUrl})";
}
