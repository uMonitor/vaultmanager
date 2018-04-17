using CommandLineParser.Exceptions;
using System;
using System.Diagnostics;
using System.IO;
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

                Trace.Listeners.Clear();

                if (options.OutputFile != null)
                {
                    Stream outputFile = File.Create(options.OutputFile);

                    TextWriterTraceListener fileListener = new TextWriterTraceListener(outputFile);
                    Trace.Listeners.Add(fileListener);
                }
                else
                {
                    Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                }

                try
                {
                    if (options.Read)
                    {
                        var cred = WindowsCredentialManager.ReadCredential(options.TargetName);
                        Trace.WriteLine(cred);
                        return;
                    }

                    if (options.Write)
                    {
                        WindowsCredentialManager.WriteCredentials(options.TargetName, options.UserName, options.Password, options.CredentialPersitence);
                        return;
                    }

                    if (options.Enumerate)
                    {
                        var credentials = WindowsCredentialManager.EnumerateCredentials();
                        foreach (var cred in credentials)
                        {
                            Trace.WriteLine(cred);
                        }

                        return;
                    }
                }
                catch(Exception ex)
                {
                    Trace.WriteLine("Exception occured");
                    Trace.WriteLine(ex.StackTrace);
                    Trace.WriteLine(ex.Message);
                }

            }
            catch(CommandLineException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
