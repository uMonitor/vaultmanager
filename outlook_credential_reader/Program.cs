using System;
using System.Text;
using System.Text.RegularExpressions;
using vaultsharp;

namespace outlook_credential_reader
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = WindowsCredentialManager.EnumerateCredentials();
            var outlookAccounts = credentials.FindAll(credential => Regex.Match(credential.TargetName, @"MicrosoftOffice\d\d_Data:SSPI:*").Success);

            foreach(var outlookAccount in outlookAccounts)
            {
                var secret = outlookAccount.GetSecret(data => Encoding.Unicode.GetString(data));
                Console.WriteLine($"{outlookAccount.TargetName}: {secret}");
            }
        }
    }
}
