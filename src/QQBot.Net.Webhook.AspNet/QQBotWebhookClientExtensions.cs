using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using QQBot.Net.Webhooks.AspNet;

namespace QQBot.Webhook.AspNet;

/// <summary>
///     提供 QQ Bot ASP.NET Webhook 的原生依赖注入和端点扩展。
/// </summary>
public static class QQBotWebhookClientExtensions
{
    /// <summary>
    ///     注册 QQ Bot ASP.NET Webhook 客户端并使用委托配置它。
    /// </summary>
    /// <param name="services">服务集合。</param>
    /// <param name="configure">配置委托。</param>
    /// <returns>服务集合。</returns>
    public static IServiceCollection AddQQBotAspNetWebhookClient(
        this IServiceCollection services,
        Action<QQBotAspNetWebhookConfig> configure)
    {
        services.AddOptions<QQBotAspNetWebhookConfig>().Configure(configure);
        services.AddSingleton(provider => new QQBotAspNetWebhookClient(
            provider.GetRequiredService<IOptions<QQBotAspNetWebhookConfig>>()));
        services.AddSingleton<QQBotWebhookClient>(provider =>
            provider.GetRequiredService<QQBotAspNetWebhookClient>());
        services.AddSingleton<IQQBotClient>(provider =>
            provider.GetRequiredService<QQBotAspNetWebhookClient>());
        services.AddSingleton<IHostedService>(provider =>
            provider.GetRequiredService<QQBotAspNetWebhookClient>());
        return services;
    }

    /// <summary>
    ///     注册使用指定配置的 QQ Bot ASP.NET Webhook 客户端。
    /// </summary>
    /// <param name="services">服务集合。</param>
    /// <param name="config">客户端配置。</param>
    /// <returns>服务集合。</returns>
    public static IServiceCollection AddQQBotAspNetWebhookClient(
        this IServiceCollection services,
        QQBotAspNetWebhookConfig config)
    {
        services.AddSingleton(Options.Create(config));
        services.AddSingleton(config);
        services.AddSingleton(provider => new QQBotAspNetWebhookClient(
            provider.GetRequiredService<IOptions<QQBotAspNetWebhookConfig>>()));
        services.AddSingleton<QQBotWebhookClient>(provider =>
            provider.GetRequiredService<QQBotAspNetWebhookClient>());
        services.AddSingleton<IQQBotClient>(provider =>
            provider.GetRequiredService<QQBotAspNetWebhookClient>());
        services.AddSingleton<IHostedService>(provider =>
            provider.GetRequiredService<QQBotAspNetWebhookClient>());
        return services;
    }

    /// <summary>
    ///     将 QQ Bot Webhook POST 端点映射到 ASP.NET 路由。
    /// </summary>
    /// <param name="endpoints">端点路由生成器。</param>
    /// <param name="routePattern">可选路由模式；未指定时使用配置值。</param>
    /// <returns>端点路由生成器。</returns>
    public static IEndpointRouteBuilder MapQQBotWebhook(
        this IEndpointRouteBuilder endpoints,
        string? routePattern = null)
    {
        QQBotAspNetWebhookClient client = endpoints.ServiceProvider
            .GetRequiredService<QQBotAspNetWebhookClient>();
        if (client.ApiClient.WebhookClient is not IAspNetWebhookClient transport)
            throw new InvalidOperationException("The configured Webhook transport is not an ASP.NET transport.");

        string route = routePattern
            ?? endpoints.ServiceProvider.GetRequiredService<IOptions<QQBotAspNetWebhookConfig>>().Value.RoutePattern;
        endpoints.MapPost(route, transport.HandleRequestAsync);
        return endpoints;
    }
}
