using System;
using System.Runtime.InteropServices;
using vaultsharp;
using vaultsharp.native;

namespace vaultmanager
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentialTarget = "MicrosoftOffice16_Data:SSPI:mapitestuser@gsx.com";

            IntPtr creds;

            //Class1.EnumerateCredentials();

            //var success = CredentialManagerWrapper.CredRead(credentialTarget, CredentialType.CRED_TYPE_GENERIC, 0, out creds);
            //var c = (NativeCredential)Marshal.PtrToStructure(creds, typeof(NativeCredential));

            //string applicationName = Marshal.PtrToStringUni(c.TargetName);
            //string userName = Marshal.PtrToStringUni(c.UserName);
            //string secret = null;
            //if (c.CredentialBlob != IntPtr.Zero)
            //{
            //    secret = Marshal.PtrToStringUni(c.CredentialBlob, (int)c.CredentialBlobSize / 2);
            //}


            WindowsCredentialManager.WriteCredentials(credentialTarget, "", "plop");
        }
    }
}
