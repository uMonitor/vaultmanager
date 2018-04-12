using CommandLineParser.Arguments;
using CommandLineParser.Validation;

namespace vaultmanager
{
    [ArgumentGroupCertification("r,w", EArgumentGroupCondition.ExactlyOneUsed)]
    [ArgumentRequiresOtherArgumentsCertification("r", "t")]
    [ArgumentRequiresOtherArgumentsCertification("w", "t,u,p")]
    class Options
    {
        [SwitchArgument('r', "read", false)]
        public bool Read;

        [SwitchArgument('w', "write", false)]
        public bool Write;

        [ValueArgument(typeof(string), 't', "target", Description = "Application Name")]
        public string TargetName;

        [ValueArgument(typeof(string), 'u', "username", Description = "Username")]
        public string UserName;

        [ValueArgument(typeof(string), 'p', "password", Description = "Password")]
        public string Password;
    }
}
