using System.Text.Json.Serialization;

namespace QQBot.API;

internal class User
{
    [JsonPropertyName("id")]
    public required ulong Id { get; init; }

    [JsonPropertyName("username")]
    public required string Username { get; init; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; init; }

    [JsonPropertyName("bot")]
    public bool? IsBot { get; init; }

    [JsonPropertyName("union_openid")]
    public string? UnionOpenId { get; init; }

    [JsonPropertyName("union_user_account")]
    public string? UnionUserAccount { get; init; }
}
