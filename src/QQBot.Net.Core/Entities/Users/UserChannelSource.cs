namespace QQBot;

/// <summary>
///     表示一个 <see cref="IUserChannel"/> 的来源。
/// </summary>
/// <remarks>
///     来源于用户添加机器人为好友到消息列表时的场景。
/// </remarks>
public enum UserChannelSource
{
    /// <summary>
    ///     缺省默认值。
    /// </summary>
    Default = 1000,

    /// <summary>
    ///     网络搜索（全部tab）。
    /// </summary>
    UserSearch = 1001,

    /// <summary>
    ///     网络搜索（机器人tab）。
    /// </summary>
    BotSearch = 1002,

    /// <summary>
    ///     群场景。
    /// </summary>
    Group = 1003,

    /// <summary>
    ///     空间场景。
    /// </summary>
    Space = 1004,

    /// <summary>
    ///     站内分享资料页。
    /// </summary>
    InternalProfileShare = 2001,

    /// <summary>
    ///     站外分享资料页。
    /// </summary>
    ExternalProfileShare = 2002,

    /// <summary>
    ///     开发者生成的分享链接（站内）。
    /// </summary>
    /// <seealso cref="QQBot.IQQBotClient.GenerateProfileUrlAsync(System.String,QQBot.RequestOptions)"/>
    InternalDeveloperShareUrl = 2003,

    /// <summary>
    ///     开发者生成的分享链接（站外）。
    /// </summary>
    /// <seealso cref="QQBot.IQQBotClient.GenerateProfileUrlAsync(System.String,QQBot.RequestOptions)"/>
    ExternalDeveloperShareUrl = 2004
}
