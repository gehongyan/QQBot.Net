// See https://aka.ms/new-console-template for more information

using QQBot;
using QQBot.Rest;
using QQBot.WebSocket;

QQBotSocketClient client = new(new QQBotSocketConfig
{
    LogLevel = LogSeverity.Debug
});
client.Log += x => Task.Run(() => Console.WriteLine(x));
client.MessageReceived += async message =>
{
    if (message.Source is not MessageSource.User) return;
    FileAttachment attachment =
        new(new Uri("https://i0.hdslb.com/bfs/garb/f8d2355a8077875db0f596c5b1b02e44e209bfed.png"));
    await message.ReplyAsync(attachment: attachment);
};
await client.LoginAsync(0, TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(Timeout.Infinite);
