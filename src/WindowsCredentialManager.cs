using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace vaultsharp
{
    public class WindowsCredentialManager
    {
        public static void WriteCredentials(string applicationName, string userName, string secret)
        {
            var credential = new CredentialWrapper(applicationName, userName, secret);

            var nativeCredential = credential.Native;
            var writeStatus = native.CredentialManagerWrapper.CredWrite(ref nativeCredential, 0);

            if (!writeStatus)
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new Exception($"CredWrite failed with the error code {lastError}.");
            }
        }

        public static Credential ReadCredential(string applicationName)
        {
            var credentialPtr = IntPtr.Zero;

            try
            {
                var readStatus = native.CredentialManagerWrapper.CredRead(applicationName, native.CredentialType.Generic, 0, out credentialPtr);

                if (!readStatus)
                {
                    int lastError = Marshal.GetLastWin32Error();
                    throw new Exception($"CredRead failed with the error code {lastError}.");
                }

                var nativeCredential = (native.NativeCredential)Marshal.PtrToStructure(credentialPtr, typeof(native.NativeCredential));
                var credential = CredentialWrapper.Convert(nativeCredential);

                return credential;
            }
            finally
            {
                native.CredentialManagerWrapper.CredFree(credentialPtr);
            }
        }

        public static List<Credential> EnumerateCredentials()
        {
            var result = new List<Credential>();

            bool enumerateStatus = native.CredentialManagerWrapper.CredEnumerate(null, 0, out int count, out IntPtr pCredentials);

            if (enumerateStatus)
            {
                for (int n = 0; n < count; n++)
                {
                    IntPtr credentialPtr = Marshal.ReadIntPtr(pCredentials, n * Marshal.SizeOf(typeof(IntPtr)));
                    var credential = CredentialWrapper.Convert((native.NativeCredential)Marshal.PtrToStructure(credentialPtr, typeof(native.NativeCredential)));

                    result.Add(credential);
                }
            }
            else
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new Exception($"Enumerate {lastError}");
            }

            return result;
        }
    }
}
