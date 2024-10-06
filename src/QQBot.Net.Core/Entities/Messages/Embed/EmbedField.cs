using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 <see cref="QQBot.IEmbed"/> 的字段。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly record struct EmbedField
{
    /// <summary>
    ///     获取此字段的名称。
    /// </summary>
    public string? Name { get; }

    internal EmbedField(string? name)
    {
        Name = name;
    }

    private string DebuggerDisplay => $"{Name}";
}
