namespace QQBot.Net.Webhooks.HttpListener;

internal interface IHttpListenerWebhookClient : IWebhookClient
{
    event Func<Exception, Task>? Closed;

    Task StartAsync(IEnumerable<string> uriPrefixes, CancellationToken cancellationToken = default);

    Task StopAsync();
}
