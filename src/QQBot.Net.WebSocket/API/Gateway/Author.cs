using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class Author
{
    [JsonPropertyName("id")]
    [GuidJsonConverter]
    public required Guid Id { get; set; }

    [JsonPropertyName("user_openid")]
    [GuidJsonConverter]
    public Guid? UserOpenId { get; set; }

    [JsonPropertyName("union_openid")]
    [GuidJsonConverter]
    public Guid? UnionOpenId { get; set; }
}
