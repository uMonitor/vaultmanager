<img src="https://raw.githubusercontent.com/Bhaal22/vaultmanager/master/resources/icon.png" width="48">

https://www.nuget.org/packages/vaultmanager/

# vaultsharp
  * .net API to manage access to WindowsCredentialManager. Wrapper implementation around native apis.


# WindowsCredentialManager class

```c#
public enum CredentialPersistence: uint
{
    Session = 1,
    LocalMachine = 2,
    Enterprise = 3
}
    
public static void WriteCredentials(string applicationName, string userName, string secret, int credentialPersistence);
public static Credential ReadCredential(string applicationName);
public static List<Credential> EnumerateCredentials();
```


# List credentials

```c#
var credentials = WindowsCredentialManager.EnumerateCredentials();
foreach (var cred in credentials)
{
    Trace.WriteLine(cred);
}
```

# Add a credential to the credential manager

```c#
var credential = WindowsCredentialManager.ReadCredential("git:https://github.com");
Trace.WriteLine(credential);
```

Note, the password is written out as base64 string.

# Write a credential

```c#
WindowsCredentialManager.WriteCredential("applicationName",
                                         "username", "password", 
                                         (int)CredentialPersistence.LocalMachine);
```

Note, that password encryption is up to the caller to set it. If password is set in crlear, then it can be read in clear.
