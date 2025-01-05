using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class SendAttachmentResponse
{
    [JsonPropertyName("file_uuid")]
    public required string FileUuid { get; init; }

    [JsonPropertyName("file_info")]
    public required string FileInfo { get; init; }

    [JsonPropertyName("ttl")]
    public required int TimeToLive { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }
}
