using CommandLineParser.Exceptions;
using System;
using vaultsharp;

namespace vaultmanager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();
                var parser = new CommandLineParser.CommandLineParser();

                parser.ExtractArgumentAttributes(options);
                parser.ParseCommandLine(args);

                if (options.Read)
                {
                    var cred = WindowsCredentialManager.ReadCredential(options.TargetName);
                    Console.WriteLine(cred);
                    return;
                }

                if (options.Write)
                {
                    WindowsCredentialManager.WriteCredentials(options.TargetName, options.UserName, options.Password);
                }
            }
            catch(CommandLineException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
