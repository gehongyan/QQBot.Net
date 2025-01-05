namespace QQBot.API.Rest;

internal class GetGuildRoleMembersParams
{
    public string? StartIndex { get; init; }
    public int? Limit { get; init; }
}
