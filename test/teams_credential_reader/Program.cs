using System;
using System.Runtime.InteropServices;
using System.Text;
using vaultsharp;

namespace teams_credential_reader
{
    class Program
    {
        static void Main(string[] args)
        {
            var teamsRootCredential = WindowsCredentialManager.ReadCredential("msteams_adalsso/adal_context_segments");
            var teamsRootSecret = teamsRootCredential.GetSecret(data => Encoding.UTF8.GetString(data));

            var segments = Convert.ToInt16(teamsRootSecret);

            var authenticationContext = string.Empty;
            for(int segmentIndex = 0; segmentIndex <= segments; segmentIndex++)
            {
                var currentSegmentCredential = WindowsCredentialManager.ReadCredential($"msteams_adalsso/adal_context_{segmentIndex}");
                var currentSegmentSecret = currentSegmentCredential.GetSecret(data => Encoding.ASCII.GetString(data));

                authenticationContext += currentSegmentSecret;
            }
        }
    }
}
