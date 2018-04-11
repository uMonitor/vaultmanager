using System;
using System.Text;

namespace vaultsharp
{
    internal class SecretManager
    {
        public static byte[] GetSecretAsByteArray(string secret)
        {
            byte[] byteArray = secret == null ? null : Encoding.Unicode.GetBytes(secret);
            // XP and Vista: 512; 
            // 7 and above: 5*512
            if (Environment.OSVersion.Version < new Version(6, 1) /* Windows 7 */)
            {
                if (byteArray != null && byteArray.Length > 512)
                    throw new ArgumentOutOfRangeException("secret", "The secret message has exceeded 512 bytes.");
            }
            else
            {
                if (byteArray != null && byteArray.Length > 512 * 5)
                    throw new ArgumentOutOfRangeException("secret", "The secret message has exceeded 2560 bytes.");
            }

            return byteArray;
        }
    }
}
