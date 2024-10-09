namespace QQBot;

internal static class IdUtils
{
    public static string ToIdString(this ulong id) => id.ToString();

    public static string ToIdString(this Guid id) => id.ToString("N").ToUpperInvariant();
}
