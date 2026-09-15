using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class PrepareMediaUploadResponse
{
    [JsonPropertyName("upload_id")]
    public required string UploadId { get; init; }
    [JsonPropertyName("block_size")]
    public required string BlockSize { get; init; }
    [JsonPropertyName("parts")]
    public required PrepareMediaUploadPart[] Parts { get; init; }
    [JsonPropertyName("upload_config")]
    public required PrepareMediaUploadConfiguration UploadConfiguration { get; init; }
}