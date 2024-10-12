namespace QQBot;

/// <summary>
///     表示一个通用的子频道内用户。
/// </summary>
public interface IGuildUser : IUser, IEntity<ulong>
{
    /// <summary>
    ///     获取此用户的唯一标识符。
    /// </summary>
    new ulong Id { get; }

    /// <summary>
    ///     获取此用户的用户名。
    /// </summary>
    string Username { get; }

    /// <summary>
    ///     获取此用户是否为 Bot。
    /// </summary>
    bool? IsBot { get; }

    /// <summary>
    ///     获取此用户特殊关联应用的 openid。
    /// </summary>
    /// <remarks>
    ///     此字段需要特殊申请并配置后才会返回。如需申请，请联系平台运营人员。
    /// </remarks>
    string? UnionOpenId { get; }

    /// <summary>
    ///     获取此用户机器人关联的互联应用的用户信息。
    /// </summary>
    /// <remarks>
    ///     此字段与 <see cref="IGuildUser.UnionOpenId"/> 关联的应用是同一个。如需申请，请联系平台运营人员。
    /// </remarks>
    string? UnionUserAccount { get; }
}
