using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class Attachment
{
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    [JsonPropertyName("content_type")]
    public required string ContentType { get; init; }

    [JsonPropertyName("filename")]
    public required string Filename { get; init; }

    [JsonPropertyName("height")]
    public int? Height { get; init; }

    [JsonPropertyName("size")]
    public required int Size { get; init; }

    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("width")]
    public int? Width { get; init; }
}
