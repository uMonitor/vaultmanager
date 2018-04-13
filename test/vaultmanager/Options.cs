using CommandLineParser.Arguments;
using CommandLineParser.Validation;

namespace vaultmanager
{
    [ArgumentGroupCertification("r,w,e", EArgumentGroupCondition.ExactlyOneUsed)]
    [ArgumentRequiresOtherArgumentsCertification("r", "t")]
    [ArgumentRequiresOtherArgumentsCertification("w", "t,u,p")]
    class Options
    {
        [SwitchArgument('r', "read", false)]
        public bool Read;

        [SwitchArgument('w', "write", false)]
        public bool Write;

        [SwitchArgument('e', "enumerate", false)]
        public bool Enumerate;

        [ValueArgument(typeof(string), 't', "target", Description = "Application Name")]
        public string TargetName;

        [ValueArgument(typeof(string), 'u', "username", Description = "Username")]
        public string UserName;

        [ValueArgument(typeof(string), 'p', "password", Description = "Password")]
        public string Password;

        [ValueArgument(typeof(string), 'o', "output-file", Description = "Output File")]
        public string OutputFile;
    }
}
