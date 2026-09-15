#nullable disable
using QQBot.Webhook.Net.ED25519.Ed25519Ref10;
using System;
using System.Security.Cryptography;

namespace QQBot.Webhook.Net.ED25519
{
    internal class Ed25519Operations
    {
        public static bool crypto_sign_verify(
            byte[] sig, int sigoffset,
            byte[] m, int moffset, int mlen,
            byte[] pk, int pkoffset)
        {
            byte[] h;
            byte[] checkr = new byte[32];
            GroupElementP3 A;
            GroupElementP2 R;

            if ((sig[sigoffset + 63] & 224) != 0)
                return false;
            if (GroupOperations.ge_frombytes_negate_vartime(out A, pk, pkoffset) != 0)
                return false;

            using IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA512);
            hasher.AppendData(sig, sigoffset, 32);
            hasher.AppendData(pk, pkoffset, 32);
            hasher.AppendData(m, moffset, mlen);
            h = hasher.GetHashAndReset();

            ScalarOperations.sc_reduce(h);

            var sm32 = new byte[32];
            Array.Copy(sig, sigoffset + 32, sm32, 0, 32);
            GroupOperations.ge_double_scalarmult_vartime(out R, h, ref A, sm32);
            GroupOperations.ge_tobytes(checkr, 0, ref R);
            var result = CryptoBytes.ConstantTimeEquals(checkr, 0, sig, sigoffset, 32);
            CryptoBytes.Wipe(h);
            CryptoBytes.Wipe(checkr);
            return result;
        }
    }
}
