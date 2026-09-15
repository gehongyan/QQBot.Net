#nullable disable
using System;
using System.Security.Cryptography;

namespace QQBot.Webhook.Net.ED25519.Ed25519Ref10
{
    internal static partial class Ed25519Operations
    {
        public static void crypto_sign(
            byte[] sig, int sigoffset,
            byte[] m, int moffset, int mlen,
            byte[] sk, int skoffset)
        {
            byte[] az, r, hram;
            GroupElementP3 R;
            using IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA512);
            {
                hasher.AppendData(sk, skoffset, 32);
                az = hasher.GetHashAndReset();
                ScalarOperations.sc_clamp(az, 0);

                hasher.AppendData(az, 32, 32);
                hasher.AppendData(m, moffset, mlen);
                r = hasher.GetHashAndReset();

                ScalarOperations.sc_reduce(r);
                GroupOperations.ge_scalarmult_base(out R, r, 0);
                GroupOperations.ge_p3_tobytes(sig, sigoffset, ref R);

                hasher.AppendData(sig, sigoffset, 32);
                hasher.AppendData(sk, skoffset + 32, 32);
                hasher.AppendData(m, moffset, mlen);
                hram = hasher.GetHashAndReset();

                ScalarOperations.sc_reduce(hram);
                var s = new byte[32];//todo: remove allocation
                Array.Copy(sig, sigoffset + 32, s, 0, 32);
                ScalarOperations.sc_muladd(s, hram, az, r);
                Array.Copy(s, 0, sig, sigoffset + 32, 32);
                CryptoBytes.Wipe(s);
            }
        }
    }
}
