using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using vaultsharp.native;

namespace vaultsharp
{
    public class WindowsCredentialManager
    {
        public static void WriteCredentials(string applicationName, string userName, string secret, int credentialPersistence)
        {
            var credential = new CredentialWrapper(applicationName, userName, secret, credentialPersistence);

            var nativeCredential = credential.Native;
            var writeStatus = CredentialManagerWrapper.CredWrite(ref nativeCredential, 0);

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
                var readStatus = CredentialManagerWrapper.CredRead(applicationName, CredentialType.Generic, 0, out credentialPtr);

                if (!readStatus)
                {
                    int lastError = Marshal.GetLastWin32Error();
                    throw new CredentialManagerException((ErrorCode)lastError);
                }

                var nativeCredential = (NativeCredential)Marshal.PtrToStructure(credentialPtr, typeof(NativeCredential));
                var credential = CredentialWrapper.Convert(nativeCredential);

                return credential;
            }
            finally
            {
                CredentialManagerWrapper.CredFree(credentialPtr);
            }
        }

        public static void DeleteCredential(string applicationName, CredentialType type = CredentialType.Generic)
        {
            var deleteStatus = CredentialManagerWrapper.CredDelete(applicationName, type, 0);

            if (!deleteStatus)
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new CredentialManagerException((ErrorCode)lastError);
            }

        }

        public static List<Credential> EnumerateCredentials()
        {
            var result = new List<Credential>();

            bool enumerateStatus = CredentialManagerWrapper.CredEnumerate(null, 0, out int count, out IntPtr pCredentials);

            if (enumerateStatus)
            {
                for (int n = 0; n < count; n++)
                {
                    IntPtr credentialPtr = Marshal.ReadIntPtr(pCredentials, n * Marshal.SizeOf(typeof(IntPtr)));
                    var credential = CredentialWrapper.Convert((NativeCredential)Marshal.PtrToStructure(credentialPtr, typeof(NativeCredential)));

                    result.Add(credential);
                }
            }
            else
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new CredentialManagerException((ErrorCode)lastError);
            }

            return result;
        }
    }
}
