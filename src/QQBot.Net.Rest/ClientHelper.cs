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
}
