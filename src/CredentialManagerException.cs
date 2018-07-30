using System;
using vaultsharp.native;

namespace vaultsharp
{
    public class CredentialManagerException: Exception
    {
        public ErrorCode ErrorCode { get; internal set; }

        public CredentialManagerException(ErrorCode errorCode)
            : base(GetMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        private static string GetMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.INVALID_FLAGS:
                    return "A flag that is not valid was specified for the Flags parameter, or CRED_ENUMERATE_ALL_CREDENTIALS is specified for the Flags parameter and the Filter parameter is not NULL.";
                case ErrorCode.NOT_FOUND:
                    return "No credential exists matching the specified Filter";
                case ErrorCode.NO_SUCH_LOGON_SESSION:
                    return "The logon session does not exist or there is no credential set associated with this logon session. Network logon sessions do not have an associated credential set.";
            }

            throw new ArgumentException($"ErrorCode {errorCode} not yet supported for string translation");
        }
    }
}
