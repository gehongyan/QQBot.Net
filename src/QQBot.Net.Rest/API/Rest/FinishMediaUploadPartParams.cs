using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class FinishMediaUploadPartParams
{
    [JsonPropertyName("upload_id")]
    public required string UploadId { get; init; }
    [JsonPropertyName("part_index")]
    public required int PartIndex { get; init; }
    [JsonPropertyName("block_size")]
    public required string BlockSize { get; init; }
    [JsonPropertyName("md5")]
    public required string Md5 { get; init; }
}