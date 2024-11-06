using System.Diagnostics;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestChannel : RestEntity<string>, IChannel
{
    internal RestChannel(BaseQQBotClient client, string id)
        : base(client, id)
    {
    }

    private string DebuggerDisplay => $"Unknown ({Id}, Channel)";

    #region IChannel

    /// <inheritdoc />
    Task<IUser?> IChannel.GetUserAsync(string id, CacheMode mode, RequestOptions? options) =>
        Task.FromResult<IUser?>(null);

    #endregion
}
