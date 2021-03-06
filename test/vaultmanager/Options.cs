﻿using CommandLineParser.Arguments;
using CommandLineParser.Validation;
using vaultsharp.native;

namespace vaultmanager
{
    [ArgumentGroupCertification("r,w,e,d", EArgumentGroupCondition.ExactlyOneUsed)]
    [ArgumentRequiresOtherArgumentsCertification("r", "t")]
    [ArgumentRequiresOtherArgumentsCertification("w", "t,u,p")]
    [ArgumentRequiresOtherArgumentsCertification("d", "t")]
    class Options
    {
        [SwitchArgument('r', "read", false)]
        public bool Read;

        [SwitchArgument('w', "write", false)]
        public bool Write;

        [SwitchArgument('e', "enumerate", false)]
        public bool Enumerate;

        [SwitchArgument('d', "delete", false)]
        public bool Delete;

        [ValueArgument(typeof(string), 't', "target", Description = "Application Name")]
        public string TargetName;

        [ValueArgument(typeof(string), 'u', "username", Description = "Username")]
        public string UserName;

        [ValueArgument(typeof(string), 'p', "password", Description = "Password")]
        public string Password;

        [ValueArgument(typeof(string), 'o', "output-file", Description = "Output File")]
        public string OutputFile;

        [EnumeratedValueArgument(typeof(int), 'c', "credential-persistence", 
            Description = "Credential persistence(Session=1, LocalMachine=2, Enterprise=3)", 
            DefaultValue = 2, AllowedValues = "1;2;3")]
        public int CredentialPersitence;
    }
}
