using vaultsharp.native;

namespace vaultsharp
{
    public class Credential
    {
        private NativeCredential _nativeCredential;

        public CredentialType Type;
        public string TargetName;
        public CredentialPersistance Persist { get; set; }
        public UInt32 AttributeCount;
        public IntPtr Attributes;
        public IntPtr TargetAlias;
        public IntPtr UserName;
    }
}
