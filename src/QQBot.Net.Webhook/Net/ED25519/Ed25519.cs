namespace QQBot.Webhook.Net.ED25519;

/// <summary>
///     提供纯托管 Ed25519 密钥派生、签名及验签功能。
/// </summary>
internal static class Ed25519
{
    public const int PublicKeySize = 32;
    public const int SignatureSize = 64;
    public const int PrivateKeySeedSize = 32;
    public const int ExpandedPrivateKeySize = 64;

    public static void CreateKeyPairFromSeed(byte[] seed, out byte[] publicKey, out byte[] expandedPrivateKey)
    {
        ArgumentNullException.ThrowIfNull(seed);
        if (seed.Length != PrivateKeySeedSize)
            throw new ArgumentException($"The seed must be {PrivateKeySeedSize} bytes.", nameof(seed));

        publicKey = new byte[PublicKeySize];
        expandedPrivateKey = new byte[ExpandedPrivateKeySize];
        Ed25519Ref10.Ed25519Operations.crypto_sign_keypair(
            publicKey, 0, expandedPrivateKey, 0, seed, 0);
    }

    public static byte[] Sign(byte[] message, byte[] expandedPrivateKey)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(expandedPrivateKey);
        if (expandedPrivateKey.Length != ExpandedPrivateKeySize)
            throw new ArgumentException(
                $"The expanded private key must be {ExpandedPrivateKeySize} bytes.", nameof(expandedPrivateKey));

        byte[] signature = new byte[SignatureSize];
        Ed25519Ref10.Ed25519Operations.crypto_sign(
            signature, 0, message, 0, message.Length, expandedPrivateKey, 0);
        return signature;
    }

    public static bool Verify(byte[] signature, byte[] message, byte[] publicKey)
    {
        ArgumentNullException.ThrowIfNull(signature);
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(publicKey);
        if (signature.Length != SignatureSize)
            return false;
        if (publicKey.Length != PublicKeySize)
            throw new ArgumentException($"The public key must be {PublicKeySize} bytes.", nameof(publicKey));

        return Ed25519Operations.crypto_sign_verify(
            signature, 0, message, 0, message.Length, publicKey, 0);
    }
}
