using System.Text;
using Microsoft.AspNetCore.Http;

namespace QQBot.Net.Webhooks.AspNet;

internal sealed class AspNetWebhookClient : IAspNetWebhookClient
{
    public event Func<WebhookRequest, Task<WebhookResponse>>? Request;

    public async Task HandleRequestAsync(HttpContext context)
    {
        using StreamReader reader = new(
            context.Request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);
        string body = await reader.ReadToEndAsync(context.RequestAborted).ConfigureAwait(false);
        WebhookRequest request = new(
            body,
            context.Request.Headers["X-Signature-Ed25519"].FirstOrDefault(),
            context.Request.Headers["X-Signature-Timestamp"].FirstOrDefault(),
            context.Request.Headers["X-Bot-Appid"].FirstOrDefault());

        WebhookResponse response = await HandleRequestAsync(request).ConfigureAwait(false);
        context.Response.StatusCode = (int)response.StatusCode;
        if (response.Body is not null)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(response.Body, context.RequestAborted).ConfigureAwait(false);
        }
    }

    public Task<WebhookResponse> HandleRequestAsync(WebhookRequest request) =>
        Request is not null
            ? Request(request)
            : Task.FromResult(WebhookResponse.NoContent);

    public void Dispose()
    {
    }
}
