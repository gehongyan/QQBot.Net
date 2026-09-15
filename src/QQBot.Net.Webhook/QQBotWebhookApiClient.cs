using System.Net;
using System.Text.Json;
using QQBot.API.Gateway;
using QQBot.API.Webhook;
using QQBot.Net.Rest;
using QQBot.Net.Webhooks;
using QQBot.Net.WebSockets;
using QQBot.Webhook.Net.ED25519;

namespace QQBot.API;

internal sealed class QQBotWebhookApiClient : QQBotSocketApiClient
{
    public event Func<Task> WebhookValidation
    {
        add => _webhookValidation.Add(value);
        remove => _webhookValidation.Remove(value);
    }

    private readonly AsyncEvent<Func<Task>> _webhookValidation = new();
    private readonly QQBotWebhookSignature _signature;

    internal IWebhookClient WebhookClient { get; }

    public QQBotWebhookApiClient(
        RestClientProvider restClientProvider,
        WebSocketProvider webSocketProvider,
        WebhookProvider webhookProvider,
        AccessEnvironment accessEnvironment,
        string userAgent,
        string secret,
        string? url = null,
        RetryMode defaultRetryMode = RetryMode.AlwaysRetry,
        Func<IRateLimitInfo, Task>? defaultRatelimitCallback = null)
        : base(restClientProvider, webSocketProvider, accessEnvironment, userAgent, url,
            defaultRetryMode, defaultRatelimitCallback: defaultRatelimitCallback)
    {
        _signature = new QQBotWebhookSignature(secret);
        WebhookClient = webhookProvider();
        WebhookClient.Request += OnRequestAsync;
    }

    private async Task<WebhookResponse> OnRequestAsync(WebhookRequest request)
    {
        if (!_signature.Verify(request.Signature, request.Timestamp, request.Body))
            return new WebhookResponse(HttpStatusCode.Unauthorized);

        GatewaySocketFrame? frame;
        try
        {
            frame = JsonSerializer.Deserialize<GatewaySocketFrame>(request.Body, _serializerOptions);
        }
        catch (JsonException)
        {
            return new WebhookResponse(HttpStatusCode.BadRequest);
        }

        if (frame is null)
            return new WebhookResponse(HttpStatusCode.BadRequest);

        try
        {
            switch (frame.OpCode)
            {
                case GatewayOpCode.CallbackValidation:
                    return await HandleValidationAsync(frame.Payload).ConfigureAwait(false);
                case GatewayOpCode.Heartbeat:
                    return CreateAck((int)GatewayOpCode.HeartbeatAck, GetInt32(frame.Payload));
                case GatewayOpCode.Dispatch:
                    await _receivedGatewayEvent.InvokeAsync(
                        frame.OpCode, frame.Sequence, frame.Type, frame.EventId, frame.Payload).ConfigureAwait(false);
                    return CreateAck((int)GatewayOpCode.HttpCallbackAck, 0);
                default:
                    await _receivedGatewayEvent.InvokeAsync(
                        frame.OpCode, frame.Sequence, frame.Type, frame.EventId, frame.Payload).ConfigureAwait(false);
                    return WebhookResponse.NoContent;
            }
        }
        catch
        {
            if (frame.OpCode == GatewayOpCode.Dispatch)
                return CreateAck((int)GatewayOpCode.HttpCallbackAck, 1);
            return new WebhookResponse(HttpStatusCode.InternalServerError);
        }
    }

    private async Task<WebhookResponse> HandleValidationAsync(object? payload)
    {
        ValidationRequest? validation = payload switch
        {
            JsonElement element => element.Deserialize<ValidationRequest>(_serializerOptions),
            _ => null
        };
        if (validation is null || string.IsNullOrEmpty(validation.PlainToken)
            || string.IsNullOrEmpty(validation.EventTimestamp))
            return new WebhookResponse(HttpStatusCode.BadRequest);

        await _webhookValidation.InvokeAsync().ConfigureAwait(false);
        ValidationResponse response = new()
        {
            PlainToken = validation.PlainToken,
            Signature = _signature.Sign(validation.EventTimestamp, validation.PlainToken)
        };
        return Json(HttpStatusCode.OK, response);
    }

    private WebhookResponse CreateAck(int opCode, int data) =>
        Json(HttpStatusCode.OK, new CallbackAck { OpCode = opCode, Data = data });

    private WebhookResponse Json<T>(HttpStatusCode statusCode, T value) =>
        new(statusCode, JsonSerializer.Serialize(value, _serializerOptions));

    private static int GetInt32(object? value) => value switch
    {
        JsonElement { ValueKind: JsonValueKind.Number } element when element.TryGetInt32(out int number) => number,
        int number => number,
        _ => 0
    };

    internal override void Dispose(bool disposing)
    {
        if (disposing)
        {
            WebhookClient.Request -= OnRequestAsync;
            WebhookClient.Dispose();
            _signature.Dispose();
        }
        base.Dispose(disposing);
    }
}
