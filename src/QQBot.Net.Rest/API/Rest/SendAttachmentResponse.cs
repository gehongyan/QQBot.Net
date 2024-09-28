using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendAttachmentResponse
{
    [JsonPropertyName("file_uuid")]
    public required Guid FileUuid { get; set; }

    [JsonPropertyName("file_info")]
    public required string FileInfo { get; set; }

    [JsonPropertyName("ttl")]
    public required int TimeToLive { get; set; }

    [JsonPropertyName("id")]
    public ulong? Id { get; set; }
}
