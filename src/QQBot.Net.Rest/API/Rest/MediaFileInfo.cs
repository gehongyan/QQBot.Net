using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class MediaFileInfo
{
    [JsonPropertyName("file_info")]
    public required string FileInfo { get; set; }
}
