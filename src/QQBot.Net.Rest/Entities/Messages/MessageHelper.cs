using System.Collections.Immutable;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class MessageHelper
{
    public static Attachment CreateAttachment(API.MessageAttachment model) =>
        new(AttachmentType.File, model.Url);

    public static async Task<IUser> GetAuthorAsync(BaseQQBotClient client, IGuild? guild, API.User model)
    {
        IUser? author = guild is not null
            ? await guild.GetUserAsync(model.Id, CacheMode.CacheOnly)
            : null;
        return author ?? RestGuildUser.Create(client, model);
    }

    public static async Task DeleteAsync(IUserMessage message, BaseQQBotClient client, bool? hideTip, RequestOptions? options)
    {
        switch (message.Channel)
        {
            case IUserChannel userChannel:
                await client.ApiClient.DeleteUserMessageAsync(userChannel.Id, message.Id, options).ConfigureAwait(false);
                break;
            case IGroupChannel groupChannel:
                await client.ApiClient.DeleteGroupMessageAsync(groupChannel.Id, message.Id, options).ConfigureAwait(false);
                break;
            case ITextChannel textChannel:
                await client.ApiClient
                    .DeleteChannelMessageAsync(textChannel.Id, message.Id,
                        new DeleteChannelMessageParams { HideTip = hideTip }, options)
                    .ConfigureAwait(false);
                break;
            case IDMChannel dmChannel:
                await client.ApiClient
                    .DeleteDirectMessageAsync(dmChannel.Id, message.Id,
                        new DeleteDirectMessageParams { HideTip = hideTip }, options)
                    .ConfigureAwait(false);
                break;
            default:
                throw new NotSupportedException("Unsupported channel type.");
        }
    }

    public static ImmutableArray<ITag> ParseTags(string text, IMessageChannel channel, IGuild? guild,
        IRole? everyoneRole, IReadOnlyCollection<IGuildUser> userMentions)
    {
        ImmutableArray<ITag>.Builder tags = ImmutableArray.CreateBuilder<ITag>();
        int index = 0;

        while (true)
        {
            index = text.IndexOf('<', index);
            if (index == -1)
                break;
            int endIndex = text.IndexOf('>', index + 1);
            if (endIndex == -1)
                break;
            string content = text.Substring(index, endIndex - index + 1);

            if (MentionUtils.TryParseUser(content, out ulong userId))
            {
                IUser? mentionedUser = channel?.GetUserAsync(userId.ToString(), CacheMode.CacheOnly).GetAwaiter().GetResult() as IUser
                    ?? userMentions.FirstOrDefault(x => x.Id == userId);
                tags.Add(new Tag<string, IUser>(TagType.UserMention, index, content.Length, userId.ToIdString(), mentionedUser));
            }
            else if (MentionUtils.TryParseChannel(content, out ulong channelId))
            {
                IGuildChannel? mentionedChannel = guild?.GetChannelAsync(channelId, CacheMode.CacheOnly).GetAwaiter().GetResult();
                tags.Add(new Tag<ulong, IGuildChannel>(TagType.ChannelMention, index, content.Length, channelId, mentionedChannel));
            }
            // else if (MentionUtils.TryParseRole(content, out id))
            // {
            //     IRole mentionedRole = null;
            //     if (guild != null)
            //         mentionedRole = guild.GetRole(id);
            //     tags.Add(new Tag<IRole>(TagType.RoleMention, index, content.Length, id, mentionedRole));
            // }
            else if (Emote.TryParse(content, out Emote? emoji))
                tags.Add(new Tag<string, Emote>(TagType.Emoji, index, content.Length, emoji.Id, emoji));
            else //Bad Tag
            {
                index++;
                continue;
            }
            index = endIndex + 1;
        }

        index = 0;
        while (true)
        {
            index = text.IndexOf(MentionUtils.MentionEveryone, index, StringComparison.Ordinal);
            if (index == -1)
                break;
            int? tagIndex = FindIndex(tags, index);
            if (tagIndex.HasValue)
                tags.Insert(tagIndex.Value, new Tag<uint, IRole>(TagType.EveryoneMention, index, MentionUtils.MentionEveryone.Length, 0U, everyoneRole));
            index++;
        }

        index = 0;
        while (true)
        {
            index = text.IndexOf("@everyone", index, StringComparison.Ordinal);
            if (index == -1)
                break;
            int? tagIndex = FindIndex(tags, index);
            if (tagIndex.HasValue)
                tags.Insert(tagIndex.Value, new Tag<uint, IRole>(TagType.EveryoneMention, index, "@everyone".Length, 0U, everyoneRole));
            index++;
        }

        return tags.ToImmutable();
    }

    private static int? FindIndex(IReadOnlyList<ITag> tags, int index)
    {
        int i = 0;
        for (; i < tags.Count; i++)
        {
            var tag = tags[i];
            if (index < tag.Index)
                break; //Position before this tag
        }
        if (i > 0 && index < tags[i - 1].Index + tags[i - 1].Length)
            return null; //Overlaps tag before this
        return i;
    }


}
