using System.Net;
using System.Text;

namespace QQBot.Net.Webhooks.HttpListener;

internal sealed class HttpListenerWebhookClient : IHttpListenerWebhookClient
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private System.Net.HttpListener? _listener;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _listenTask;
    private bool _isDisposed;
    private bool _isStopping;

    public event Func<WebhookRequest, Task<WebhookResponse>>? Request;
    public event Func<Exception, Task>? Closed;

    public async Task StartAsync(IEnumerable<string> uriPrefixes, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(CancellationToken.None).ConfigureAwait(false);
        try
        {
            await StopInternalAsync().ConfigureAwait(false);
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _listener = new System.Net.HttpListener();
            foreach (string prefix in uriPrefixes)
                _listener.Prefixes.Add(prefix);
            _listener.Start();
            _listenTask = RunAsync(_cancellationTokenSource.Token);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task StopAsync()
    {
        await _lock.WaitAsync(CancellationToken.None).ConfigureAwait(false);
        try
        {
            await StopInternalAsync().ConfigureAwait(false);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task StopInternalAsync()
    {
        _isStopping = true;
        try
        {
            _cancellationTokenSource?.Cancel();
            _listener?.Close();
            _listener = null;
            if (_listenTask is not null)
            {
                await _listenTask.ConfigureAwait(false);
                _listenTask = null;
            }
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
        finally
        {
            _isStopping = false;
        }
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                System.Net.HttpListener listener = _listener
                    ?? throw new InvalidOperationException("The HTTP listener is not running.");
                HttpListenerContext context = await listener.GetContextAsync().WaitAsync(cancellationToken)
                    .ConfigureAwait(false);
                await HandleRequestAsync(context, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            if (!_isStopping && Closed is not null)
                _ = NotifyClosedAsync(exception);
        }
    }

    private async Task NotifyClosedAsync(Exception exception)
    {
        await Task.Yield();
        if (Closed is not null)
            await Closed(exception).ConfigureAwait(false);
    }

    private async Task HandleRequestAsync(HttpListenerContext context, CancellationToken cancellationToken)
    {
        try
        {
            using StreamReader reader = new(
                context.Request.InputStream,
                context.Request.ContentEncoding ?? Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            string body = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            WebhookRequest request = new(
                body,
                context.Request.Headers["X-Signature-Ed25519"],
                context.Request.Headers["X-Signature-Timestamp"],
                context.Request.Headers["X-Bot-Appid"]);
            WebhookResponse response = await HandleRequestAsync(request).ConfigureAwait(false);

            context.Response.StatusCode = (int)response.StatusCode;
            if (response.Body is not null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(response.Body);
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.ContentLength64 = bytes.Length;
                await context.Response.OutputStream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
            }
            context.Response.Close();
        }
        catch
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.Close();
        }
    }

    public Task<WebhookResponse> HandleRequestAsync(WebhookRequest request) =>
        Request is not null
            ? Request(request)
            : Task.FromResult(WebhookResponse.NoContent);

    public void Dispose()
    {
        if (_isDisposed)
            return;
        StopInternalAsync().GetAwaiter().GetResult();
        _lock.Dispose();
        _isDisposed = true;
    }
}
