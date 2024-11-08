namespace QQBot;

/// <summary>
///     提供用于创建 <see cref="QQBot.IForumChannel" /> 的属性。
/// </summary>
/// <seealso cref="QQBot.IGuild.CreateForumChannelAsync(System.String,System.Action{QQBot.CreateForumChannelProperties},QQBot.RequestOptions)"/>
public class CreateForumChannelProperties : CreateNestedChannelProperties;