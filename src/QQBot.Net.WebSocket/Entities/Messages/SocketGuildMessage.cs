using System.Collections.Immutable;
using System.Diagnostics;
using QQBot.Rest;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的频道子频道或频道内用户私聊的消息。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketGuildMessage : SocketMessage, IGuildMessage
{
    private ImmutableArray<Embed> _embeds = [];

    /// <inheritdoc />
    public bool? MentionedEveryone { get; private set; }

    /// <inheritdoc />
    public IReadOnlyCollection<Embed> Embeds => _embeds;

    internal SocketGuildMessage(QQBotSocketClient client, string id,
        ISocketMessageChannel channel, SocketUser author, MessageSource source)
        : base(client, id, channel, author, source)
    {
    }

    internal static new SocketGuildMessage Create(QQBotSocketClient client, ClientState state,
        SocketUser author, ISocketMessageChannel channel, API.ChannelMessage model)
    {
        SocketGuildMessage entity = new(client, model.Id, channel, author, MessageSource.User);
        entity.Update(state, model);
        return entity;
    }

    internal override void Update(ClientState state, API.ChannelMessage model)
    {
        base.Update(state, model);
        MentionedEveryone = model.MentionEveryone;
        if (model.Embeds is { Length: > 0 } embedModels)
            _embeds = [..embedModels.Select(x => x.ToEntity())];
    }
}
