using System.Reflection;
using QQBot.Commands;
using QQBot.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace QQBot.Net.Samples.TextCommands.Services;

public class CommandHandlingService
{
    private readonly CommandService _commands;
    private readonly QQBotSocketClient _client;
    private readonly IServiceProvider _services;

    public CommandHandlingService(IServiceProvider services)
    {
        _commands = services.GetRequiredService<CommandService>();
        _client = services.GetRequiredService<QQBotSocketClient>();
        _services = services;

        // Hook CommandExecuted to handle post-command-execution logic.
        _commands.CommandExecuted += CommandExecutedAsync;
        // Hook MessageReceived so we can process each message to see
        // if it qualifies as a command.
        _client.MessageReceived += MessageReceivedAsync;
    }

    public async Task InitializeAsync()
    {
        if (Assembly.GetEntryAssembly() is not { } assembly) return;
        // Register modules that are public and inherit ModuleBase<T>.
        await _commands.AddModulesAsync(assembly, _services);
    }

    public async Task MessageReceivedAsync(SocketUserMessage message)
    {
        if (_client.CurrentUser is null) return;

        // Ignore system messages, or messages from other bots
        if (message.Source is not MessageSource.User) return;

        // This value holds the offset where the prefix ends
        int argPos = 0;
        // Perform prefix check. The default formats in various contexts are:
        if (message.Channel is IGuildChannel)
            if (!message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;
        if (message.Channel is IGroupChannel or IGuildChannel)
            if (!message.HasCharPrefix('/', ref argPos)) return;
        if (!message.HasCharPrefix('/', ref argPos)) return;
        // for a more traditional command format like !help.
        // if (!message.HasMentionPrefix(_client.CurrentUser, ref argPos))
        //     return;

        SocketCommandContext context = new(_client, message);
        // Perform the execution of the command. In this method,
        // the command service will perform precondition and parsing check
        // then execute the command if one is matched.
        await _commands.ExecuteAsync(context, argPos, _services);
        // Note that normally a result will be returned by this format, but here
        // we will handle the result in CommandExecutedAsync,
    }

    public async Task CommandExecutedAsync(CommandInfo? command, ICommandContext context, IResult result)
    {
        // command is unspecified when there was a search failure (command not found); we don't care about these errors
        if (command is null)
            return;

        // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
        if (result.IsSuccess)
            return;

        // the command failed, let's notify the user that something happened.
        await context.Message.ReplyAsync($"error: {result}");
    }
}
