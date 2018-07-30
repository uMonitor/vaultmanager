using System;
using System.Runtime.InteropServices;

namespace vaultsharp.native
{
    /***
     * https://docs.microsoft.com/en-us/windows/desktop/api/wincred/nf-wincred-credenumeratea
     ***/
    public enum ErrorCode: uint
    {
        INVALID_FLAGS = 0x3Ec,          //A flag that is not valid was specified for the Flags parameter, or CRED_ENUMERATE_ALL_CREDENTIALS is specified for the Flags parameter and the Filter parameter is not NULL.
        NOT_FOUND = 0x490,              //No credential exists matching the specified Filter
        NO_SUCH_LOGON_SESSION = 0x520   //The logon session does not exist or there is no credential set associated with this logon session. Network logon sessions do not have an associated credential set.
    }

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

    public enum CredentialPersistence: uint
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
        public CredentialPersistence Persist;
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
                    Persist = CredentialPersistence.Session
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
