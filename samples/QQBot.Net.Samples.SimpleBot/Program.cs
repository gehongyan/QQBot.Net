// See https://aka.ms/new-console-template for more information

using QQBot;
using QQBot.WebSocket;

QQBotSocketClient client = new(new QQBotSocketConfig
{
    LogLevel = LogSeverity.Debug,
    AccessEnvironment = AccessEnvironment.Sandbox,
    GatewayIntents = GatewayIntents.All
});
client.Log += x => Task.Run(() => Console.WriteLine(x));
client.MessageReceived += async message =>
{
    if (message.Source is not MessageSource.User) return;
    IUserMessage msg = await message.ReplyAsync(message.Content);
    await Task.Delay(TimeSpan.FromSeconds(1));
    await msg.DeleteAsync();
};
await client.LoginAsync(0, TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(Timeout.Infinite);
