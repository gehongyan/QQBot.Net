namespace QQBot.API.Gateway;

internal enum GatewayOpCode
{
    Dispatch = 0,
    Heartbeat = 1,
    Identify = 2,
    Resume = 6,
    Reconnect = 7,
    InvalidSession = 9,
    Hello = 10,
    HeartbeatAck = 11,
    HttpCallbackAck = 12
}
