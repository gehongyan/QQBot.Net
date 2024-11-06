using QQBot.API;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class ClientHelper
{
    public static async Task<BotGateway> GetBotGatewayAsync(BaseQQBotClient client, RequestOptions? options)
    {
        GetBotGatewayResponse response = await client.ApiClient.GetBotGatewayAsync(options).ConfigureAwait(false);
        SessionStartLimit sessionStartLimit = new()
        {
            Total = response.SessionStartLimit.Total,
            Remaining = response.SessionStartLimit.Remaining,
            ResetAfter = response.SessionStartLimit.ResetAfter,
            MaxConcurrency = response.SessionStartLimit.MaxConcurrency
        };
        return new BotGateway(response.Url, response.Shards, sessionStartLimit);
    }

    public static IAsyncEnumerable<IReadOnlyCollection<API.Guild>> GetGuildsAsync(BaseQQBotClient client, int? limit, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<API.Guild>(
            QQBotConfig.MaxMembersPerBatch,
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
                if (lastPage.LastOrDefault()?.Id is not { } lastId)
                    return false;
                info.Position = lastId;
                return true;
            },
            start: null,
            count: limit
        );
    }

    public static async Task<RestGuild> GetGuildAsync(BaseQQBotClient client, ulong id, RequestOptions? options)
    {
        Guild model= await client.ApiClient.GetGuildAsync(id, options);
        return RestGuild.Create(client, model);
    }
}
