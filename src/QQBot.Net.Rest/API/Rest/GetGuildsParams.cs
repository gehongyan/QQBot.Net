namespace QQBot.API.Rest;

internal class GetGuildsParams
{
    public ulong? BeforeId { get; set; }
    public ulong? AfterId { get; set; }
    public int? Limit { get; set; }
}
