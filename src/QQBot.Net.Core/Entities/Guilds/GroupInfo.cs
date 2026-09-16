using System.Diagnostics;

namespace QQBot;

/// <summary>
///     表示一个 QQ 群的基本信息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class GroupInfo
{
    /// <summary>
    ///     获取此群的唯一标识符。
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    ///     获取此群的名称。
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取此群的简介。
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     获取此群的分类。
    /// </summary>
    public string Category { get; }

    /// <summary>
    ///     获取此群的标签。
    /// </summary>
    public IReadOnlyCollection<string> Tags { get; }

    /// <summary>
    ///     获取此群的成员数量。
    /// </summary>
    public int MemberCount { get; }

    internal GroupInfo(Guid id, string name, string description, string category,
        IReadOnlyCollection<string> tags, int memberCount)
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
        Tags = tags;
        MemberCount = memberCount;
    }

    private string DebuggerDisplay => $"{Name} ({Id}, {MemberCount} Members)";
}
