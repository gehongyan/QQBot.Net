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
    ArkBuilder arkBuilder = new(24);
    arkBuilder.WithParameter("#DESC#", "描述");
    arkBuilder.WithParameter("#PROMPT#", "通知");
    arkBuilder.WithParameter("#TITLE#", "标题");
    arkBuilder.WithParameter("#METADESC#", "描述信息");
    arkBuilder.WithParameter("#SUBTITLE#", "子标题");
    await message.ReplyAsync(ark: arkBuilder.Build());
};
await client.LoginAsync(0, TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(Timeout.Infinite);
