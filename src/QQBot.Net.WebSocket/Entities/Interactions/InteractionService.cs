using System.Collections.Concurrent;

namespace QQBot.WebSocket;

/// <summary>
///     提供互动事件的路由服务。
/// </summary>
public class InteractionService
{
    private readonly ConcurrentDictionary<string, Func<SocketInteraction, Task>> _buttonIdHandlers =
        new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, Func<SocketInteraction, Task>> _buttonDataHandlers =
        new(StringComparer.Ordinal);

    /// <summary>
    ///     注册一个按按钮 ID 匹配的处理程序。
    /// </summary>
    /// <param name="buttonId"> 要匹配的按钮 ID。 </param>
    /// <param name="handler"> 匹配成功时要执行的处理程序。 </param>
    /// <exception cref="ArgumentException"> 指定的按钮 ID 已注册。 </exception>
    public void RegisterButtonHandler(string buttonId, Func<SocketInteraction, Task> handler)
    {
        Preconditions.NotNullOrWhiteSpace(buttonId, nameof(buttonId));
        Preconditions.NotNull(handler, nameof(handler));
        if (!_buttonIdHandlers.TryAdd(buttonId, handler))
            throw new ArgumentException("A handler is already registered for this button ID.", nameof(buttonId));
    }

    /// <summary>
    ///     注册一个按按钮回调数据匹配的处理程序。
    /// </summary>
    /// <param name="buttonData"> 要匹配的按钮回调数据。 </param>
    /// <param name="handler"> 匹配成功时要执行的处理程序。 </param>
    /// <exception cref="ArgumentException"> 指定的按钮回调数据已注册。 </exception>
    public void RegisterButtonDataHandler(string buttonData, Func<SocketInteraction, Task> handler)
    {
        Preconditions.NotNullOrWhiteSpace(buttonData, nameof(buttonData));
        Preconditions.NotNull(handler, nameof(handler));
        if (!_buttonDataHandlers.TryAdd(buttonData, handler))
            throw new ArgumentException("A handler is already registered for this button data.", nameof(buttonData));
    }

    /// <summary>
    ///     移除按按钮 ID 注册的处理程序。
    /// </summary>
    /// <param name="buttonId"> 已注册的按钮 ID。 </param>
    /// <returns> 如果成功移除处理程序，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public bool RemoveButtonHandler(string buttonId) => _buttonIdHandlers.TryRemove(buttonId, out _);

    /// <summary>
    ///     移除按按钮回调数据注册的处理程序。
    /// </summary>
    /// <param name="buttonData"> 已注册的按钮回调数据。 </param>
    /// <returns> 如果成功移除处理程序，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public bool RemoveButtonDataHandler(string buttonData) => _buttonDataHandlers.TryRemove(buttonData, out _);

    /// <summary>
    ///     执行与指定互动匹配的处理程序。
    /// </summary>
    /// <remarks>
    ///     服务会优先使用按钮 ID 匹配处理程序；没有匹配项时，再使用按钮回调数据匹配。
    /// </remarks>
    /// <param name="interaction"> 要路由的互动事件。 </param>
    /// <returns> 如果找到并执行了处理程序，则为 <see langword="true"/>；否则为 <see langword="false"/>。 </returns>
    public async Task<bool> ExecuteAsync(SocketInteraction interaction)
    {
        Preconditions.NotNull(interaction, nameof(interaction));
        if (interaction.Type is not InteractionType.MessageButton)
            return false;

        Func<SocketInteraction, Task>? handler = null;
        if (interaction.ButtonId is { } buttonId)
            _buttonIdHandlers.TryGetValue(buttonId, out handler);
        if (handler is null && interaction.ButtonData is { } buttonData)
            _buttonDataHandlers.TryGetValue(buttonData, out handler);
        if (handler is null)
            return false;

        await handler(interaction).ConfigureAwait(false);
        return true;
    }
}
