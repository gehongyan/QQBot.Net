using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示模板中的一个单值参数。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly record struct ArkSingleParameter : IArkParameter
{
    /// <summary>
    ///     获取参数值。
    /// </summary>
    public string Value { get; }

    internal ArkSingleParameter(string value)
    {
        Value = value;
    }

    private string DebuggerDisplay => $"Value: {Value}";
}
