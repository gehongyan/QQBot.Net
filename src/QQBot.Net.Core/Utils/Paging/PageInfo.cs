namespace QQBot;

internal class PageInfo
{
    public int Page { get; set; }
    public ulong? Position { get; set; }
    public int? Count { get; set; }
    public int PageSize { get; set; }
    public int? Remaining { get; set; }
    public string? Cookie { get; set; }

    internal PageInfo(ulong? position, int? count, int pageSize)
    {
        Page = 1;
        Position = position;
        Count = count;
        Remaining = count;
        PageSize = pageSize;
        if (Count < PageSize)
            PageSize = Count.Value;
    }
}
