using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个空元素。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class EmptyElement : IElement
{
    /// <inheritdoc />
    public ElementType Type => ElementType.Empty;

    internal EmptyElement()
    {
    }

    private string DebuggerDisplay => "Empty";
}
