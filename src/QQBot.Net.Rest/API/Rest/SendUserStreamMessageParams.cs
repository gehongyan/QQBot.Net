using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendUserStreamMessageParams
{
    [JsonPropertyName("input_mode")]
    public required string InputMode { get; init; }

    [JsonPropertyName("input_state")]
    public required StreamMessageInputState InputState { get; init; }

    [JsonPropertyName("index")]
    public required int Index { get; init; }

    [JsonPropertyName("content_type")]
    public required string ContentType { get; init; }

    [JsonPropertyName("content_raw")]
    public required string Content { get; init; }

    [JsonPropertyName("event_id")]
    public string? EventId { get; init; }

    [JsonPropertyName("msg_id")]
    public string? MessageId { get; init; }

    [JsonPropertyName("stream_msg_id")]
    public string? StreamMessageId { get; init; }

    [JsonPropertyName("msg_seq")]
    public int? MessageSequence { get; init; }

    [JsonPropertyName("is_wakeup")]
    public bool? IsWakeup { get; init; }
}
