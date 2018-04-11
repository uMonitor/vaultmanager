using System;
using System.Runtime.InteropServices;

namespace vaultsharp
{
    public class WindowsCredentialManager
    {
        public static void WriteCredentials(string applicationName, string userName, string secret)
        {
            var byteArray = SecretManager.GetSecretAsByteArray(secret);
            var credential = native.NativeCredential.Default;

            credential.CredentialBlobSize = (uint)(byteArray == null ? 0 : byteArray.Length);
            credential.TargetName = Marshal.StringToCoTaskMemUni(applicationName);
            credential.CredentialBlob = Marshal.StringToCoTaskMemUni(secret);
            credential.UserName = Marshal.StringToCoTaskMemUni(userName ?? Environment.UserName);

            var writeStatus = native.CredentialManagerWrapper.CredWrite(ref credential, 0);

            if (!writeStatus)
            {

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
