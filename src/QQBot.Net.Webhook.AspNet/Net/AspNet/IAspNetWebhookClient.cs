using Microsoft.AspNetCore.Http;

namespace QQBot.Net.Webhooks.AspNet;

internal interface IAspNetWebhookClient : IWebhookClient
{
    Task HandleRequestAsync(HttpContext context);
}
