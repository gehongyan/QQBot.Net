using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageReference
{
    [JsonPropertyName("message_id")]
    public required string MessageId { get; init; }

    [JsonPropertyName("ignore_get_message_error")]
    public bool? IgnoreGetMessageError { get; init; }
}
