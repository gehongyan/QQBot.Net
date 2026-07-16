using QQBot.API.Rest;

namespace QQBot.WebSocket;

internal static class SocketInteractionHelper
{
    public static async Task SendMessageAsync(SocketInteraction interaction, string content,
        RequestOptions? options)
    {
        Preconditions.NotNullOrWhiteSpace(content, nameof(content));
        if (string.IsNullOrWhiteSpace(interaction.EventId))
            throw new NotSupportedException("The interaction does not contain a gateway event ID.");

        switch (interaction.Scene)
        {
            case InteractionScene.C2C when interaction.UserOpenId.HasValue:
                await interaction.Client.ApiClient.SendUserMessageAsync(
                        interaction.UserOpenId.Value, CreateUserGroupMessageParams(interaction, content), options)
                    .ConfigureAwait(false);
                break;
            case InteractionScene.Group when interaction.GroupOpenId.HasValue:
                await interaction.Client.ApiClient.SendGroupMessageAsync(
                        interaction.GroupOpenId.Value, CreateUserGroupMessageParams(interaction, content), options)
                    .ConfigureAwait(false);
                break;
            case InteractionScene.Guild when interaction.ChannelId.HasValue:
                SendChannelMessageParams args = new()
                {
                    Content = content,
                    EventId = interaction.EventId
                };
                await interaction.Client.ApiClient.SendChannelMessageAsync(interaction.ChannelId.Value, args, options)
                    .ConfigureAwait(false);
                break;
            default:
                throw new NotSupportedException("The interaction does not contain a supported message context.");
        }
    }

    private static SendUserGroupMessageParams CreateUserGroupMessageParams(SocketInteraction interaction,
        string content) => new()
    {
        Content = content,
        MessageType = MessageType.Text,
        EventId = interaction.EventId
    };
}
