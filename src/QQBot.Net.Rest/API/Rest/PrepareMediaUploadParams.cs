using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class PrepareMediaUploadParams
{
    [JsonPropertyName("file_type")]
    public required AttachmentType FileType { get; init; }
    [JsonPropertyName("file_size")]
    public required string FileSize { get; init; }
    [JsonPropertyName("file_name")]
    public required string FileName { get; init; }
    [JsonPropertyName("md5")]
    public required string Md5 { get; init; }
    [JsonPropertyName("sha1")]
    public required string Sha1 { get; init; }
    [JsonPropertyName("md5_10m")]
    public required string Md5First10MiB { get; init; }
}
