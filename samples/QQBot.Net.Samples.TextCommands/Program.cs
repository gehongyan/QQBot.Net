using QQBot;
using QQBot.Commands;
using QQBot.Net.Samples.TextCommands.Services;
using QQBot.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// 这是一个使用 QQBot.Net 的文本命令框架的简单示例

// 此处使用了 .NET 通用主机承载 KOOK Bot 服务，该主机将帮助我们管理应用程序与服务的依赖注入与生命周期。
// 有关 .NET 通用主机的更多信息，请参阅 https://learn.microsoft.com/dotnet/core/extensions/generic-host
// 如果您使用另一个依赖注入框架，应该查阅其文档以找到最佳的处理方式。

// 您可以在以下位置找到使用命令框架的文档：
// - https://kooknet.dev/guides/text_commands/intro.html

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<QQBotSocketConfig>(_ => new QQBotSocketConfig
{
    LogLevel = LogSeverity.Debug,
    AccessEnvironment = AccessEnvironment.Sandbox,
    GatewayIntents = GatewayIntents.All
});
builder.Services.AddSingleton<QQBotSocketClient>(provider =>
{
    QQBotSocketConfig config = provider.GetRequiredService<QQBotSocketConfig>();
    return new QQBotSocketClient(config);
});
builder.Services.AddSingleton<CommandService>();
builder.Services.AddSingleton<CommandHandlingService>();
builder.Services.AddHostedService<QQBotClientService>();
builder.Services.AddSingleton<PictureService>();
builder.Services.AddHttpClient("Pictures");

IHost app = builder.Build();
await app.RunAsync();
