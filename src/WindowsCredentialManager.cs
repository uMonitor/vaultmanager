using System;
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

        private static void ReadCredential(native.NativeCredential credential)
        {
            string applicationName = Marshal.PtrToStringUni(credential.TargetName);
            string userName = Marshal.PtrToStringUni(credential.UserName);
            string secret = null;
            if (credential.CredentialBlob != IntPtr.Zero)
            {
                secret = Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
            }

            //return new Credential(credential.Type, applicationName, userName, secret);
        }

        public static void EnumerateCredentials()
        {
            //List<Credential> result = new List<Credential>();

            int count;
            IntPtr pCredentials;
            bool ret = native.CredentialManagerWrapper.CredEnumerate(null, 0, out count, out pCredentials);
            if (ret)
            {
                for (int n = 0; n < count; n++)
                {
                    IntPtr credential = Marshal.ReadIntPtr(pCredentials, n * Marshal.SizeOf(typeof(IntPtr)));
                    ReadCredential((native.NativeCredential)Marshal.PtrToStructure(credential, typeof(native.NativeCredential)));

                    //result.Add(ReadCredential((CREDENTIAL)Marshal.PtrToStructure(credential, typeof(CREDENTIAL))));
                }
            }
            else
            {
                //int lastError = Marshal.GetLastWin32Error();
                //throw new Win32Exception(lastError);
            }

            //return result;
        }
    }
}
