namespace QQBot.API.Rest;

internal class GetGuildMembersParams
{
    public ulong? AfterId { get; init; }
    public int? Limit { get; init; }
}
