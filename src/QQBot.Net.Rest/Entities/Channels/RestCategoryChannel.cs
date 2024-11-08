using System.Diagnostics;
using Model = QQBot.API.Channel;

namespace QQBot.Rest;

/// <summary>
///     表示一个基于 REST 的分组子频道。
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RestCategoryChannel : RestGuildChannel, ICategoryChannel
{
    internal RestCategoryChannel(BaseQQBotClient client, ulong id, IGuild guild)
        : base(client, id, guild)
    {
        Type = ChannelType.Category;
    }

    internal static new RestCategoryChannel Create(BaseQQBotClient client, IGuild guild, Model model)
    {
        RestCategoryChannel entity = new(client, model.Id, guild);
        entity.Update(model);
        return entity;
    }

    /// <inheritdoc />
    public async Task ModifyAsync(Action<ModifyCategoryChannelProperties> func, RequestOptions? options = null)
    {
        Model model = await ChannelHelper.ModifyAsync(this, Client, func, options).ConfigureAwait(false);
        Update(model);
    }

    private string DebuggerDisplay => $"{Name} ({Id}, Category)";
}
