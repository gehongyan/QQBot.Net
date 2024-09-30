// See https://aka.ms/new-console-template for more information

using QQBot;
using QQBot.Rest;
using QQBot.WebSocket;

QQBotSocketClient client = new(new QQBotSocketConfig
{
    LogLevel = LogSeverity.Debug
});
client.Log += x => Task.Run(() => Console.WriteLine(x));
client.MessageReceived += message =>
{
    Console.WriteLine($"Received Message [Author] {message.Author.Id} [Channel] {message.Channel.Id} [Content] {message.Content}");
    foreach (Attachment attachment in message.Attachments)
        Console.WriteLine($"Attachment [Type] {attachment.Type} [Url] {attachment.Url}");
    return Task.CompletedTask;
};
await client.LoginAsync(0, TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(Timeout.Infinite);
