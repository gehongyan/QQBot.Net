using System.Text.Json.Serialization;

namespace QQBot.API;

internal class User
{
    [JsonPropertyName("id")]
    public required ulong Id { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("bot")]
    public bool? IsBot { get; set; }

    [JsonPropertyName("union_openid")]
    public string? UnionOpenId { get; set; }

    [JsonPropertyName("union_user_account")]
    public string? UnionUserAccount { get; set; }
}
