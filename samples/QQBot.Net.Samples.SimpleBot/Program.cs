// See https://aka.ms/new-console-template for more information

using QQBot;
using QQBot.Rest;
using QQBot.WebSocket;

QQBotSocketClient client = new(new QQBotSocketConfig
{
    LogLevel = LogSeverity.Debug,
    AccessEnvironment = AccessEnvironment.Sandbox,
    GatewayIntents = GatewayIntents.All,
    StartupCacheFetchData = StartupCacheFetchData.All
});
client.Log += x => Task.Run(() => Console.WriteLine(x));
client.Ready += async () =>
{
    await Task.Yield();
    Console.WriteLine("Ready!");
};
client.MessageReceived += async message =>
{
    if (message.Source is not MessageSource.User) return;
    if (message.Channel is SocketTextChannel textChannel)
    {
        IReadOnlyCollection<ApplicationPermission> permissions = await textChannel.Guild.GetApplicationPermissionsAsync();
    }

//      IUserMessage msg = await message.ReplyAsync(
//          $"""
//          [Content] {message.Content}
//          [Content] {Format.Escape(message.Content)}
//          [Content] {Format.Sanitize(message.Content)}
//          [Attachments] {message.Attachments.Count}
//          """);
};
await client.LoginAsync(0, TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(TimeSpan.FromSeconds(10));
await Task.Delay(Timeout.Infinite);
