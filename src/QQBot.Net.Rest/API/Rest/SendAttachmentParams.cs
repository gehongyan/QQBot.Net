using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendAttachmentParams
{
    [JsonPropertyName("file_type")]
    public required AttachmentType FileType { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("srv_send_msg")]
    public required bool ServerSendMessage { get; set; }

    // [JsonPropertyName("file_data")]
    // public object? FileData { get; set; }
}
