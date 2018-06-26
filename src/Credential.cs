using System;
using System.Runtime.InteropServices;
using vaultsharp.native;

namespace vaultsharp
{
    public class Credential
    {
        public string TargetName { get; internal set; }
        public string UserName { get; internal set; }

        public byte[] Secret { get; set; }


        public T GetSecret<T>(Func<byte[], T> data_extract)
        {
            return data_extract(Secret);
        }

        public override string ToString()
        {
            var username = UserName ?? string.Empty;
            return $"{TargetName}: Username='{username}' Password={Secret}";
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

        public CredentialWrapper(string applicationName, string userName, string secret, int credentialPersistence)
        {
            _native = NativeCredential.Default;

            var byteArray = SecretManager.GetSecretAsByteArray(secret);

            _native.CredentialBlobSize = (uint)(byteArray == null ? 0 : byteArray.Length);
            _native.TargetName = Marshal.StringToCoTaskMemUni(applicationName);
            _native.CredentialBlob = Marshal.StringToCoTaskMemUni(secret);

            if (string.IsNullOrEmpty(userName))
            {
                _native.UserName = IntPtr.Zero;
            }
            else
            {
                _native.UserName = Marshal.StringToCoTaskMemUni(userName ?? Environment.UserName);
            }

            _native.Persist = (CredentialPersistence)credentialPersistence;
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
                credential.Secret = new byte[native.CredentialBlobSize];
                Marshal.Copy(native.CredentialBlob, credential.Secret, 0, (int)native.CredentialBlobSize);
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
