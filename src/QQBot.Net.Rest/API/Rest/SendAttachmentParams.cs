using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendAttachmentParams
{
    [JsonPropertyName("file_type")]
    public required AttachmentType FileType { get; init; }

    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("srv_send_msg")]
    public required bool ServerSendMessage { get; init; }

    // [JsonPropertyName("file_data")]
    // public object? FileData { get; init; }
}
