using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class OperateGroupBlacklistResponse
{
    [JsonPropertyName("fail_openids")]
    public string[]? FailOpenids { get; init; }
}
