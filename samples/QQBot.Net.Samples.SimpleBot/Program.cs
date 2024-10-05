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
    string summary = $"""
        Received Message
        [Id] {message.Id}
        [Author] {message.Author.Id}
        [Channel] {message.Channel.Id}
        [Content] {message.Content}
        """;
    Console.WriteLine(summary);
    await message.ReplyAsync(summary);
    foreach (Attachment attachment in message.Attachments)
        Console.WriteLine($"Attachment [Type] {attachment.Type} [Url] {attachment.Url}");
};
await client.LoginAsync(0, TokenType.BotToken, "");
await client.StartAsync();
await Task.Delay(Timeout.Infinite);
