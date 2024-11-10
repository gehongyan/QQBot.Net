using System.Text.Json.Serialization;

namespace QQBot.API.Rest;

internal class GetGuildRoleMembersParams
{
    public string? StartIndex { get; set; }
    public int? Limit { get; set; }
}
