using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace QQBot.Webhook.AspNet;

/// <summary>
///     表示一个由 ASP.NET Generic Host 托管的 QQ Bot Webhook 客户端。
/// </summary>
public sealed class QQBotAspNetWebhookClient : QQBotWebhookClient, IHostedService
{
    internal QQBotAspNetWebhookClient(IOptions<QQBotAspNetWebhookConfig> options)
        : base(options.Value)
    {
    }

    internal QQBotAspNetWebhookClient(QQBotAspNetWebhookConfig config)
        : base(config)
    {
    }

    internal new QQBotAspNetWebhookConfig BaseConfig => base.BaseConfig as QQBotAspNetWebhookConfig
        ?? throw new InvalidOperationException("The configuration is not an ASP.NET Webhook configuration.");

    /// <inheritdoc />
    public override Task StartAsync() =>
        throw new NotSupportedException("The hosted Webhook client does not support manual starting.");

    /// <inheritdoc />
    public override Task StopAsync() =>
        throw new NotSupportedException("The hosted Webhook client does not support manual stopping.");

    /// <inheritdoc />
    protected override async Task OnWebhookValidationAsync()
    {
        if (!BaseConfig.AutoLogin && ConnectionState == ConnectionState.Disconnected)
            await StartCoreAsync().ConfigureAwait(false);
        await base.OnWebhookValidationAsync().ConfigureAwait(false);
    }

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        if (BaseConfig.AutoLogin)
            await StartCoreAsync().ConfigureAwait(false);
    }

    private async Task StartCoreAsync()
    {
        if (!BaseConfig.AppId.HasValue)
            throw new InvalidOperationException("The bot AppId is required.");
        if (string.IsNullOrWhiteSpace(BaseConfig.Secret))
            throw new InvalidOperationException("The bot secret is required.");

        await LoginAsync(
            BaseConfig.AppId.Value,
            TokenType.AppSecret,
            BaseConfig.Secret,
            BaseConfig.ValidateToken).ConfigureAwait(false);
        await base.StartAsync().ConfigureAwait(false);
    }

    async Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync().ConfigureAwait(false);
        if (BaseConfig.AutoLogout)
            await LogoutAsync().ConfigureAwait(false);
    }
}
