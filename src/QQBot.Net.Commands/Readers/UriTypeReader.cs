using System.Text.RegularExpressions;

namespace QQBot.Commands;

internal class UriTypeReader : TypeReader
{
    private static readonly Regex ResolveMarkdownUrlRegex = new(@"^\s*\[.+?\]\((?<url>.+?)\)\s*$", RegexOptions.Compiled);

    /// <inheritdoc />
    public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    {
        string resolvedInput = ResolveMarkdownUrlRegex.Match(input) is { Success: true } match
            ? match.Groups["url"].Value
            : input;
        return Task.FromResult(Uri.TryCreate(resolvedInput, UriKind.RelativeOrAbsolute, out Uri? uri)
            ? TypeReaderResult.FromSuccess(uri)
            : TypeReaderResult.FromError(CommandError.ParseFailed, "Failed to parse Uri"));
    }
}
