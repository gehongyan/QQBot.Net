using System.Text.Json.Serialization;

namespace QQBot.API.Gateway;

internal class MessageScene
{
    private const string MessageIndexPrefix = "msg_idx=";

    [JsonPropertyName("source")]
    public string? Source { get; init; }

    [JsonPropertyName("ext")]
    public string[]? Extensions { get; init; }

    [JsonIgnore]
    public string? MessageIndex => Extensions?
        .FirstOrDefault(x => x.StartsWith(MessageIndexPrefix, StringComparison.Ordinal))?
        [MessageIndexPrefix.Length..];
}
