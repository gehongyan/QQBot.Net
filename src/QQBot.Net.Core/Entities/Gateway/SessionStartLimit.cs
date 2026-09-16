using System.Diagnostics;

namespace QQBot;

/// <summary>
///     Stores the information related to the gateway identify request.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SessionStartLimit
{
    /// <summary>
    ///     Gets the total number of session starts the current user is allowed.
    /// </summary>
    /// <returns>
    ///     The maximum amount of session starts the current user is allowed.
    /// </returns>
    public int Total { get; internal set; }
    /// <summary>
    ///     Gets the remaining number of session starts the current user is allowed.
    /// </summary>
    /// <returns>
    ///     The remaining amount of session starts the current user is allowed.
    /// </returns>
    public int Remaining { get; internal set; }
    /// <summary>
    ///     Gets the number of milliseconds after which the limit resets.
    /// </summary>
    /// <returns>
    ///     The milliseconds until the limit resets back to the <see cref="Total"/>.
    /// </returns>
    public int ResetAfter { get; internal set; }
    /// <summary>
    ///     Gets the maximum concurrent identify requests in a time window.
    /// </summary>
    /// <returns>
    ///     The maximum concurrent identify requests in a time window,
    ///     limited to the same rate limit key.
    /// </returns>
    public int MaxConcurrency { get; internal set; }

    private string DebuggerDisplay => $"{Remaining}/{Total} (Resets in {ResetAfter}ms)";
}
