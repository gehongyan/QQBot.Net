# QQBot.Net ASP.NET Webhook Sample

This sample hosts the QQ Bot callback endpoint at `/qqbot` using ASP.NET Core.

The current working tree temporarily inlines the test bot AppId and secret in `Program.cs` for local debugging. **Remove those values before committing** and restore configuration-based credential loading.

Recommended environment variables after restoring secure configuration:

```powershell
$env:QQBot__AppId = "your-app-id"
$env:QQBot__Secret = "your-bot-secret"
dotnet run --project .\QQBot.Net.Samples.Webhook.AspNet.csproj
```

Configure the public callback URL in the QQ Bot management console. The public URL must use HTTPS. QQ currently allows ports 80, 443, 8080, and 8443. For local development, expose the HTTPS endpoint through a reverse proxy or tunnel that preserves the raw request body and the `X-Signature-Ed25519` and `X-Signature-Timestamp` headers.
