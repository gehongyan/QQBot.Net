﻿using QQBot.API;
using QQBot.API.Rest;

namespace QQBot.Rest;

internal static class GuildHelper
{
    #region Channels

    public static async Task<IReadOnlyCollection<RestGuildChannel>> GetChannelsAsync(IGuild guild,
        BaseQQBotClient client,
        RequestOptions? options)
    {
        IReadOnlyCollection<Channel> models =
            await client.ApiClient.GetChannelsAsync(guild.Id, options).ConfigureAwait(false);
        return [..models.Select(x => RestGuildChannel.Create(client, guild, x))];
    }

    public static async Task<RestGuildChannel> GetChannelAsync(IGuild guild, BaseQQBotClient client,
        ulong id, RequestOptions? options)
    {
        Channel model = await client.ApiClient.GetChannelAsync(id, options).ConfigureAwait(false);
        return RestGuildChannel.Create(client, guild, model);
    }

    public static async Task<RestTextChannel> CreateTextChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateTextChannelProperties>? func, RequestOptions? options)
    {
        CreateTextChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.Text,
            // Position = props.Position,
            SubType = props.SubType,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission,
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestTextChannel.Create(client, guild, model);
    }

    public static async Task<RestVoiceChannel> CreateVoiceChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateVoiceChannelProperties>? func, RequestOptions? options)
    {
        CreateVoiceChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.Voice,
            // Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission,
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestVoiceChannel.Create(client, guild, model);
    }

    public static async Task<RestLiveStreamChannel> CreateLiveStreamChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateLiveStreamChannelProperties>? func, RequestOptions? options)
    {
        CreateLiveStreamChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.LiveStream,
            // Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission,
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestLiveStreamChannel.Create(client, guild, model);
    }

    public static async Task<RestApplicationChannel> CreateApplicationChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateApplicationChannelProperties>? func, RequestOptions? options)
    {
        CreateApplicationChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.Application,
            // Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission,
            ApplicationId = props.ApplicationType,
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestApplicationChannel.Create(client, guild, model);
    }

    public static async Task<RestForumChannel> CreateForumChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateForumChannelProperties>? func, RequestOptions? options)
    {
        CreateForumChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.Forum,
            // Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission,
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestForumChannel.Create(client, guild, model);
    }

    public static async Task<RestScheduleChannel> CreateScheduleChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateScheduleChannelProperties>? func, RequestOptions? options)
    {
        CreateScheduleChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.Schedule,
            // Position = props.Position,
            PrivateType = props.PrivateType,
            SpeakPermission = props.SpeakPermission,
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestScheduleChannel.Create(client, guild, model);
    }

    public static async Task<RestCategoryChannel> CreateCategoryChannelAsync(IGuild guild, BaseQQBotClient client,
        string name, Action<CreateCategoryChannelProperties>? func, RequestOptions? options)
    {
        CreateCategoryChannelProperties props = new();
        func?.Invoke(props);
        CreateChannelParams args = new()
        {
            Name = name,
            Type = ChannelType.Category,
            Position = props.Position
        };
        Channel model = await client.ApiClient.CreateChannelAsync(guild.Id, args, options).ConfigureAwait(false);
        return RestCategoryChannel.Create(client, guild, model);
    }

    #endregion

    #region Users

    public static async Task<RestGuildMember> GetUserAsync(IGuild guild, BaseQQBotClient client,
        ulong id, RequestOptions? options)
    {
        Member model = await client.ApiClient.GetGuildMemberAsync(guild.Id, id, options).ConfigureAwait(false);
        if (model.User is null)
            throw new InvalidOperationException("User not found in guild.");
        return RestGuildMember.Create(client, guild, model.User, model);
    }

    public static IAsyncEnumerable<IReadOnlyCollection<Member>> GetMembersAsync(
        IGuild guild, BaseQQBotClient client, int? limit, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<Member>(
            QQBotConfig.MaxMembersPerBatch,
            async (info, ct) =>
            {
                GetGuildMembersParams args = new()
                {
                    Limit = info.PageSize
                };
                if (info.Position != null)
                    args.AfterId = info.Position.Value;
                return [..await client.ApiClient.GetGuildMembersAsync(guild.Id, args, options).ConfigureAwait(false)];
            },
            nextPage: (info, lastPage) =>
            {
                if (lastPage.LastOrDefault()?.User?.Id is not { } lastId)
                    return false;
                info.Position = lastId;
                return true;
            },
            start: null,
            count: limit
        );
    }

    public static IAsyncEnumerable<IReadOnlyCollection<IGuildMember>> GetUsersAsync(
        IGuild guild, BaseQQBotClient client, int? limit, RequestOptions? options)
    {
        return new PagedAsyncEnumerable<IGuildMember>(
            QQBotConfig.MaxMembersPerBatch,
            async (info, ct) =>
            {
                GetGuildMembersParams args = new()
                {
                    Limit = info.PageSize
                };
                if (info.Position != null)
                    args.AfterId = info.Position.Value;
                IReadOnlyCollection<Member> models = await client.ApiClient
                    .GetGuildMembersAsync(guild.Id, args, options).ConfigureAwait(false);
                return
                [
                    ..models.Select(x => RestGuildMember.Create(
                        client, guild, x.User ?? throw new InvalidOperationException("User not found in guild."), x))
                ];
            },
            nextPage: (info, lastPage) =>
            {
                if (lastPage.LastOrDefault()?.Id is not { } lastId)
                    return false;
                info.Position = lastId;
                return true;
            },
            start: null,
            count: limit
        );
    }

    #endregion

    #region Roles

    public static async Task<IReadOnlyCollection<RestRole>> GetRolesAsync(IGuild guild,
        BaseQQBotClient client, RequestOptions? options)
    {
        GetGuildRolesResponse model =
            await client.ApiClient.GetGuildRolesAsync(guild.Id, options).ConfigureAwait(false);
        return [..model.Roles.Select(x => RestRole.Create(guild, client, x))];
    }

    public static async Task<RestRole?> GetRoleAsync(IGuild guild,
        BaseQQBotClient client, ulong id, RequestOptions? options)
    {
        GetGuildRolesResponse model =
            await client.ApiClient.GetGuildRolesAsync(guild.Id, options).ConfigureAwait(false);
        Role? role = model.Roles.FirstOrDefault(x => x.Id == id);
        return role is not null ? RestRole.Create(guild, client, role) : null;
    }

    public static async Task<RestRole> CreateRoleAsync(IGuild guild, BaseQQBotClient client,
        Action<RoleProperties>? func, RequestOptions? options)
    {
        RoleProperties properties = new();
        func?.Invoke(properties);
        CreateGuildRoleParams args = new()
        {
            Name = properties.Name,
            Color = properties.Color,
            Hoist = properties.IsHoisted
        };
        CreateGuildRoleResponse model = await client.ApiClient.CreateGuildRoleAsync(guild.Id, args, options);
        return RestRole.Create(guild, client, model.Role);
    }

    #endregion

    #region API Permissions

    public static async Task<IReadOnlyCollection<ApplicationPermission>> GetApplicationPermissionsAsync(IGuild guild,
        BaseQQBotClient client, RequestOptions? options)
    {
        IReadOnlyCollection<ApiPermission> models =
            await client.ApiClient.GetApplicationGuildPermissionsAsync(guild.Id, options);
        return [..models.Select(x => new ApplicationPermission(new HttpMethod(x.Method), x.Path, x.Description, x.AuthStatus))];
    }

    #endregion

    public static Task RequestApplicationPermissionAsync(IGuild guild, BaseQQBotClient client, ITextChannel channel,
        string title, ApplicationPermission permission, RequestOptions? options) =>
        RequestApplicationPermissionAsync(guild, client, channel.Id, title, permission, options);

    public static Task RequestApplicationPermissionAsync(IGuild guild, BaseQQBotClient client, ulong channelId,
        string title, ApplicationPermission permission, RequestOptions? options) =>
        RequestApplicationPermissionAsync(guild, client, channelId, title,
            permission.Description, permission.Method, permission.Path, options);

    public static Task RequestApplicationPermissionAsync(IGuild guild, BaseQQBotClient client, ITextChannel channel,
        string title, string description, HttpMethod method, string path, RequestOptions? options) =>
        RequestApplicationPermissionAsync(guild, client, channel.Id, title, description, method, path, options);

    public static Task RequestApplicationPermissionAsync(IGuild guild, BaseQQBotClient client, ulong channelId,
        string title, string description, HttpMethod method, string path, RequestOptions? options) =>
        ChannelHelper.RequestApplicationPermissionAsync(guild.Id, channelId, client, title, description, method, path, options);
}
