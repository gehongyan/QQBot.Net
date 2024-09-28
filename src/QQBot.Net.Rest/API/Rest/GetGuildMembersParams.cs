namespace QQBot.API.Rest;

internal class GetGuildMembersParams
{
    public ulong? AfterId { get; set; }
    public int? Limit { get; set; }
}
