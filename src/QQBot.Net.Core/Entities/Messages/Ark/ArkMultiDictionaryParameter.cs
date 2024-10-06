using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示模板中的一个多字典列表参数。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly record struct ArkMultiDictionaryParameter : IArkParameter
{
    /// <summary>
    ///     获取参数值。
    /// </summary>
    public IReadOnlyCollection<IReadOnlyDictionary<string, string>> Value { get; }

    internal ArkMultiDictionaryParameter(IReadOnlyCollection<IReadOnlyDictionary<string, string>> value)
    {
        Value = value;
    }

    private string DebuggerDisplay => $"Values Count: {Value.Count}";
}
