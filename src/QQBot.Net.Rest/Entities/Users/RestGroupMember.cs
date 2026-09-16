using System.Diagnostics;
using QQBot.API;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的 QQ 群成员。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestGroupMember : RestUser, IGroupMember
{
    /// <inheritdoc cref="QQBot.IGroupMember.Id" />
    public new Guid Id { get; }

    /// <inheritdoc />
    public string Username { get; private set; }

    /// <inheritdoc />
    public GroupMemberRole Role { get; private set; }

    /// <inheritdoc />
    public bool IsBot { get; private set; }

    /// <inheritdoc />
    public DateTimeOffset JoinedAt { get; private set; }

    /// <inheritdoc />
    public string? UnionOpenId { get; private set; }

    internal RestGroupMember(BaseQQBotClient client, Guid id)
        : base(client, id.ToIdString())
    {
        Id = id;
        Username = string.Empty;
    }

    internal static RestGroupMember Create(BaseQQBotClient client, GroupMember model)
    {
        RestGroupMember entity = new(client, model.MemberOpenId);
        entity.Update(model);
        return entity;
    }

    internal void Update(GroupMember model)
    {
        Username = model.Username ?? string.Empty;
        Role = model.MemberRole;
        IsBot = model.Bot;
        JoinedAt = model.JoinedAt;
        UnionOpenId = model.UnionOpenId;
    }

    /// <inheritdoc cref="QQBot.Rest.RestGroupMember.Username" />
    public override string ToString() => Username;

    private string DebuggerDisplay => $"{Username} ({Id}{(IsBot ? ", Bot" : "")}, GroupMember)";
}
