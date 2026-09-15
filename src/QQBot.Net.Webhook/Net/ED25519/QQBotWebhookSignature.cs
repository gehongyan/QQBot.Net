using System.Security.Cryptography;
using System.Text;

namespace QQBot.Webhook.Net.ED25519;

/// <summary>
///     按照 QQ Bot Webhook 规范生成及验证 Ed25519 签名。
/// </summary>
internal sealed class QQBotWebhookSignature : IDisposable
{
    private readonly byte[] _publicKey;
    private readonly byte[] _privateKey;
    private bool _isDisposed;

    public QQBotWebhookSignature(string secret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);
        byte[] source = Encoding.UTF8.GetBytes(secret);
        byte[] seed = new byte[Ed25519.PrivateKeySeedSize];
        for (int i = 0; i < seed.Length; i++)
            seed[i] = source[i % source.Length];

        Ed25519.CreateKeyPairFromSeed(seed, out _publicKey, out _privateKey);
        CryptographicOperations.ZeroMemory(seed);
        CryptographicOperations.ZeroMemory(source);
    }

    public bool Verify(string? signature, string? timestamp, string body)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        if (string.IsNullOrWhiteSpace(signature) || string.IsNullOrWhiteSpace(timestamp))
            return false;

        byte[] signatureBytes;
        try
        {
            signatureBytes = Convert.FromHexString(signature);
        }
        catch (FormatException)
        {
            return false;
        }

        byte[] message = GetSignedContent(timestamp, body);
        try
        {
            return Ed25519.Verify(signatureBytes, message, _publicKey);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(message);
            CryptographicOperations.ZeroMemory(signatureBytes);
        }
    }

    public string Sign(string timestamp, string body)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
        ArgumentException.ThrowIfNullOrWhiteSpace(timestamp);
        ArgumentNullException.ThrowIfNull(body);

        byte[] message = GetSignedContent(timestamp, body);
        byte[]? signature = null;
        try
        {
            signature = Ed25519.Sign(message, _privateKey);
            return Convert.ToHexString(signature).ToLowerInvariant();
        }
        finally
        {
            CryptographicOperations.ZeroMemory(message);
            if (signature is not null)
                CryptographicOperations.ZeroMemory(signature);
        }
    }

    private static byte[] GetSignedContent(string timestamp, string body) =>
        Encoding.UTF8.GetBytes(string.Concat(timestamp, body));

    public void Dispose()
    {
        if (_isDisposed)
            return;
        CryptographicOperations.ZeroMemory(_privateKey);
        CryptographicOperations.ZeroMemory(_publicKey);
        _isDisposed = true;
    }
}
