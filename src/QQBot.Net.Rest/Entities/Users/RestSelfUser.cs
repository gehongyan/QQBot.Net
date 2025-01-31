﻿using System.Diagnostics;
using Model = QQBot.API.SelfUser;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的当前用户。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestSelfUser : RestGuildUser, ISelfUser
{
    /// <inheritdoc cref="QQBot.Rest.RestGuildUser.Id" />
    public new ulong Id { get; }

    /// <inheritdoc />
    internal RestSelfUser(BaseQQBotClient client, ulong id)
        : base(client, id)
    {
        Id = id;
    }

    internal static RestSelfUser Create(BaseQQBotClient client, Model model)
    {
        RestSelfUser entity = new(client, model.Id);
        entity.Update(model);
        return entity;
    }

    internal void Update(Model model)
    {
        base.Update(model);
    }

    private string DebuggerDisplay =>
        $"{Username} ({Id}{(IsBot ?? false ? ", Bot" : "")}, Self)";
}
