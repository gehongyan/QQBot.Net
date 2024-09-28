using System.Text.Json.Serialization;

namespace QQBot.API;

internal class MessageEmbed
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    [JsonPropertyName("thumbnail")]
    public MessageEmbedThumbnail? Thumbnail { get; set; }

    [JsonPropertyName("fields")]
    public MessageEmbedField[]? Fields { get; set; }
}

internal class MessageEmbedThumbnail
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

internal class MessageEmbedField
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
