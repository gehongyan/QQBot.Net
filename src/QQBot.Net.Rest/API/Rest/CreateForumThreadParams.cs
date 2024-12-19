using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateForumThreadParams
{
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }

    [JsonPropertyName("format")]
    public required ThreadTextType Format { get; set; }
}

internal class CreateForumThreadResponse
{

}
