using QQBot.API;
using QQBot.API.Rest;

namespace QQBot.WebSocket;

internal static class SocketClientHelper
{
    public static IAsyncEnumerable<IReadOnlyCollection<Guild>> GetGuildsAsync(
        QQBotSocketClient client, int? limit, RequestOptions options)
    {
        return new PagedAsyncEnumerable<Guild>(
            QQBotConfig.MaxGuildsPerBatch,
            async (info, ct) =>
            {
                GetGuildsParams args = new()
                {
                    Limit = info.PageSize
                };
                if (info.Position != null)
                    args.AfterId = info.Position.Value;
                return [..await client.ApiClient.GetGuildsAsync(args, options).ConfigureAwait(false)];
            },
            nextPage: (info, lastPage) =>
            {
                if (lastPage.Count != QQBotConfig.MaxGuildsPerBatch)
                    return false;
                info.Position = lastPage.Last().Id;
                return true;
            },
            start: null,
            count: limit
        );
    }
}
