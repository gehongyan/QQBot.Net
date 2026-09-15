using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendAttachmentParams
{
    [JsonPropertyName("file_type")]
    public required AttachmentType FileType { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }

    [JsonPropertyName("srv_send_msg")]
    public required bool ServerSendMessage { get; init; }

    [JsonPropertyName("file_name")]
    public string? FileName { get; init; }

    [JsonPropertyName("upload_id")]
    public string? UploadId { get; init; }

    // [JsonPropertyName("file_data")]
    // public object? FileData { get; init; }
}
