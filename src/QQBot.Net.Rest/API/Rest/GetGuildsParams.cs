namespace QQBot.API.Rest;

internal class GetGuildsParams
{
    public ulong? BeforeId { get; init; }
    public ulong? AfterId { get; init; }
    public int? Limit { get; init; }
}
