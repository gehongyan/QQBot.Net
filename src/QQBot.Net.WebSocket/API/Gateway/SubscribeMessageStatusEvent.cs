using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class SubscribeMessageStatusEvent
{
    [JsonPropertyName("group_openid")]
    [GuidJsonConverter]
    public Guid? GroupOpenid { get; init; }

    [JsonPropertyName("openid")]
    [GuidJsonConverter]
    public Guid? Openid { get; init; }

    [JsonPropertyName("result")]
    public required SubscribeMessageTemplateResult[] Result { get; init; }
}

internal class SubscribeMessageTemplateResult
{
    [JsonPropertyName("template_id")]
    public int TemplateId { get; init; }

    [JsonPropertyName("custom_template_id")]
    public string? CustomTemplateId { get; init; }

    [JsonPropertyName("op")]
    public int Op { get; init; }

    [JsonPropertyName("subscribe_id")]
    public string? SubscribeId { get; init; }

    [JsonPropertyName("update_ts")]
    [DateTimeOffsetTimestampJsonConverter(Unit = DateTimeOffsetTimestampJsonConverter.Format.Seconds)]
    public DateTimeOffset UpdateTs { get; init; }
}
