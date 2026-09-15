using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class PrepareMediaUploadPart
{
    [JsonPropertyName("index")]
    public required int Index { get; init; }
    [JsonPropertyName("presigned_url")]
    public required string PresignedUrl { get; init; }
    [JsonPropertyName("block_size")]
    public required string BlockSize { get; init; }
}