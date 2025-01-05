using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class CreateForumThreadParams
{
    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("content")]
    public required string Content { get; init; }

    [JsonPropertyName("format")]
    public required ThreadTextType Format { get; init; }
}

internal class CreateForumThreadResponse
{

}
