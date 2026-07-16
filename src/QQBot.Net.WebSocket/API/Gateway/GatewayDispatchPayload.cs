namespace QQBot.API.Gateway;

internal record GatewayDispatchPayload(string? EventId, object Payload);
