using System.Diagnostics;
using QQBot.Rest;
using Model = QQBot.API.Channel;

namespace QQBot.WebSocket;

/// <summary>
///     表示一个基于网关的分组子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class SocketCategoryChannel : SocketGuildChannel, ICategoryChannel
{
    internal SocketCategoryChannel(QQBotSocketClient client, ulong id, SocketGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Category;
    }

    internal static new SocketCategoryChannel Create(SocketGuild guild, ClientState state, Model model)
    {
        SocketCategoryChannel entity = new(guild.Client, model.Id, guild);
        entity.Update(state, model);
        return entity;
    }

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyCategoryChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(Client.State, model);
    }

    private string DebuggerDisplay => $"{Name} ({Id}, Category)";
}
