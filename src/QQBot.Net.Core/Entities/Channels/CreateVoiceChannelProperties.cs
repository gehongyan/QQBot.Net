namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.IVoiceChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateVoiceChannelAsync(System.String,System.Action{QQBot.CreateVoiceChannelProperties},QQBot.RequestOptions)"/>
public class CreateVoiceChannelProperties : CreateNestedChannelProperties;