using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class Attachment
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("content_type")]
    public required string ContentType { get; set; }

    [JsonPropertyName("filename")]
    public required string Filename { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("size")]
    public required int Size { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }
}