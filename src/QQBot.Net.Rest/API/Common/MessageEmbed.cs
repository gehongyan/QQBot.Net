using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageEmbed
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("prompt")]
    public string? Prompt { get; init; }

    [JsonPropertyName("thumbnail")]
    public MessageEmbedThumbnail? Thumbnail { get; init; }

    [JsonPropertyName("fields")]
    public MessageEmbedField[]? Fields { get; init; }
}

internal class MessageEmbedThumbnail
{
    [JsonPropertyName("url")]
    public string? Url { get; init; }
}

internal class MessageEmbedField
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
