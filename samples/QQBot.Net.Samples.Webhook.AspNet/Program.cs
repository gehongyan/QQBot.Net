using System.Net;
using QQBot;
using QQBot.Webhook.AspNet;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string certificateDirectory = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "certificate", "*"));
string certificatePath = Path.Combine(certificateDirectory, "*.pfx");
string certificatePasswordPath = Path.Combine(certificateDirectory, "*.txt");
if (!File.Exists(certificatePath) || !File.Exists(certificatePasswordPath))
    throw new FileNotFoundException("The local HTTPS certificate or its password file was not found.");
string certificatePassword = (await File.ReadAllTextAsync(certificatePasswordPath))?.Trim()
    ?? throw new InvalidOperationException("The local HTTPS certificate password file is empty.");

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Loopback, 5043, listenOptions =>
        listenOptions.UseHttps(certificatePath, certificatePassword));
});

const int appId = 0;
const string secret = "*";
string routePattern = builder.Configuration["QQBot:RoutePattern"] ?? "/qqbot";

builder.Services.AddQQBotAspNetWebhookClient(new QQBotAspNetWebhookConfig
{
    AppId = appId,
    Secret = secret,
    RoutePattern = routePattern,
    LogLevel = LogSeverity.Debug
});

WebApplication app = builder.Build();
QQBotAspNetWebhookClient client = app.Services.GetRequiredService<QQBotAspNetWebhookClient>();
client.Log += message =>
{
    Console.WriteLine(message);
    return Task.CompletedTask;
};
client.Ready += () =>
{
    Console.WriteLine("QQ Bot Webhook client is ready.");
    return Task.CompletedTask;
};
client.MessageReceived += message =>
{
    Console.WriteLine($"Message received: {message.Id}");
    return Task.CompletedTask;
};

app.MapGet("/", () => Results.Ok(new
{
    Status = "running",
    Webhook = routePattern
}));
app.MapQQBotWebhook(routePattern);

await app.RunAsync();
