using QQBot.Rest;

namespace QQBot.WebSocket;

internal static class SocketInteractionHelper
{
    public static async Task<IUserMessage> SendMessageAsync(SocketInteraction interaction, string? content,
        IMarkdown? markdown, FileAttachment? attachment, Embed? embed, Ark? ark, IKeyboard? keyboard,
        MessageReference? messageReference, RequestOptions? options)
    {
        if (string.IsNullOrWhiteSpace(interaction.EventId))
            throw new NotSupportedException("The interaction does not contain a gateway event ID.");

        return interaction.Scene switch
        {
            InteractionScene.C2C when interaction.UserOpenId.HasValue =>
                await ChannelHelper.SendMessageAsync(GetUserChannel(interaction), interaction.Client,
                        content, markdown, attachment, embed, ark, keyboard, messageReference, null,
                        interaction.EventId, options)
                    .ConfigureAwait(false),
            InteractionScene.Group when interaction.GroupOpenId.HasValue =>
                await ChannelHelper.SendMessageAsync(GetGroupChannel(interaction), interaction.Client,
                        content, markdown, attachment, embed, ark, keyboard, messageReference, null,
                        interaction.EventId, options)
                    .ConfigureAwait(false),
            InteractionScene.Guild when interaction.GuildId.HasValue && interaction.ChannelId.HasValue =>
                await SendGuildMessageAsync(interaction, content, markdown, attachment, embed, ark, keyboard,
                        messageReference, options)
                    .ConfigureAwait(false),
            _ => throw new NotSupportedException(
                "The interaction does not contain a supported message context.")
        };
    }

    private static IUserChannel GetUserChannel(SocketInteraction interaction) =>
        interaction.Channel as IUserChannel ??
        new SocketUserChannel(interaction.Client, interaction.UserOpenId!.Value, interaction.User);

    private static IGroupChannel GetGroupChannel(SocketInteraction interaction) =>
        interaction.Channel as IGroupChannel ??
        new SocketGroupChannel(interaction.Client, interaction.GroupOpenId!.Value);

    private static async Task<IUserMessage> SendGuildMessageAsync(SocketInteraction interaction,
        string? content, IMarkdown? markdown, FileAttachment? attachment, Embed? embed, Ark? ark,
        IKeyboard? keyboard, MessageReference? messageReference, RequestOptions? options)
    {
        if (keyboard is not null)
            throw new NotSupportedException("Cannot send a keyboard to a guild text channel.");

        ITextChannel channel = interaction.Channel as ITextChannel ??
            new RestTextChannel(interaction.Client, interaction.ChannelId!.Value,
                interaction.Guild ?? new SocketGuild(interaction.Client, interaction.GuildId!.Value));
        return await ChannelHelper.SendMessageAsync(channel, interaction.Client, content, markdown,
                attachment, embed, ark, messageReference, null, interaction.EventId, options)
            .ConfigureAwait(false);
    }
}
