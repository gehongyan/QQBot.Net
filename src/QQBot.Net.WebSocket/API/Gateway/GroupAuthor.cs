using System.Text.Json.Serialization;
using QQBot.Net.Converters;

namespace QQBot.API.Gateway;

internal class GroupAuthor
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("member_openid")]
    [GuidJsonConverter]
    public Guid? MemberOpenId { get; set; }

    [JsonPropertyName("union_openid")]
    [GuidJsonConverter]
    public Guid? UnionOpenId { get; set; }
}
