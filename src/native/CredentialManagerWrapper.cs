using System;
using System.Runtime.InteropServices;

namespace vaultsharp.native
{
    public enum CredentialType: uint
    {
        Generic = 1,
        DomainPassword,
        DomainCertificate,
        DomainVisiblePassword,
        GenericCertificate,
        DomainExtended,
        Maximum,
        MaximumEx = Maximum + 1000
    }

    public enum CredentialPersistance: uint
    {
        Session = 1,
        LocalMachine = 2,
        Enterprise = 3
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct NativeCredential
    {
        public UInt32 Flags;
        public CredentialType Type;
        public IntPtr TargetName;
        public IntPtr Comment;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
        public UInt32 CredentialBlobSize;
        public IntPtr CredentialBlob;
        public CredentialPersistance Persist;
        public UInt32 AttributeCount;
        public IntPtr Attributes;
        public IntPtr TargetAlias;
        public IntPtr UserName;

        public static NativeCredential Default
        {
            get
            {
                var defaultCredential = new NativeCredential()
                {
                    AttributeCount = 0,
                    Attributes = IntPtr.Zero,
                    Comment = IntPtr.Zero,
                    TargetAlias = IntPtr.Zero,
                    Type = CredentialType.Generic,
                    Persist = CredentialPersistance.LocalMachine
                };

                return defaultCredential;
            }
        }
    }

    internal class CredentialManagerWrapper
    {
        [DllImport("advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredDelete(string target, CredentialType type, int reservedFlag);

        [DllImport("advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredRead(string target, CredentialType type, int reservedFlag, out IntPtr CredentialPtr);

        [DllImport("advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredWrite([In] ref NativeCredential userCredential, [In] UInt32 flags);

        [DllImport("advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        internal static extern bool CredFree([In] IntPtr cred);

        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool CredEnumerate(string filter, int flag, out int count, out IntPtr credentials);
    }
}
