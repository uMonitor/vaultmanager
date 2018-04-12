using System;
using System.Runtime.InteropServices;
using vaultsharp.native;

namespace vaultsharp
{
    public class Credential
    {
        public string TargetName { get; internal set; }
        public string UserName { get; internal set; }
        public string Secret { get; internal set; }

        public override string ToString()
        {
            return $"{TargetName}: Username={UserName} - Password={Secret}";
        }
    }

    internal class CredentialWrapper
    {
        private NativeCredential _native;
        public NativeCredential Native
        {
            get
            {
                return _native;
            }
        }

        public Credential Credential { get; internal set; }

        public CredentialWrapper(string applicationName, string userName, string secret)
        {
            _native = NativeCredential.Default;

            var byteArray = SecretManager.GetSecretAsByteArray(secret);

            _native.CredentialBlobSize = (uint)(byteArray == null ? 0 : byteArray.Length);
            _native.TargetName = Marshal.StringToCoTaskMemUni(applicationName);
            _native.CredentialBlob = Marshal.StringToCoTaskMemUni(secret);
            _native.UserName = Marshal.StringToCoTaskMemUni(userName ?? Environment.UserName);
        }

        public static Credential Convert(NativeCredential native)
        {
            var credential = new Credential
            {
                UserName = Marshal.PtrToStringUni(native.UserName),
                TargetName = Marshal.PtrToStringUni(native.TargetName)
            };

            if (native.CredentialBlob != IntPtr.Zero)
            {
                credential.Secret = Marshal.PtrToStringUni(native.CredentialBlob, (int)native.CredentialBlobSize / 2);
            }

            return credential;
        }

        ~CredentialWrapper()
        {
            Marshal.FreeCoTaskMem(Native.TargetName);
            Marshal.FreeCoTaskMem(Native.CredentialBlob);
            Marshal.FreeCoTaskMem(Native.UserName);
        }
    }
}
