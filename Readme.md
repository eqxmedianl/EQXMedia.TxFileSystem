# EQXMedia.TxFileSystem

`EQXMedia.TxFileSystem` is a transactional filesystem wrapper using the .NET Standard filesystem abstraction from `System.IO.Abstractions`.

This filesystem wrapper supports transactional operations on:
*   Files,
*   Directories,
*   Filestreams.

# NuGet Package

A NuGet package is created of every `EQXMedia.TxFileSystem` release and can be installed to your .NET project using the *NuGet Package Manager* or `dotnet` command.
More information about the NuGet package can be found at NuGet.org:
[https://www.nuget.org/packages/EQXMedia.TxFileSystem/](https://www.nuget.org/packages/EQXMedia.TxFileSystem/ "Visit the NuGet package listing of EQXMedia.TxFileSystem")

## Installing the package

### Package Manager
```powershell
PS> Install-Package EQXMedia.TxFileSystem -Version 1.0.0
```

### .NET CLI
```
$ dotnet add package EQXMedia.TxFileSystem --version 1.0.0
```

### Package Reference
```xml
<PackageReference Include="EQXMedia.TxFileSystem" Version="1.0.0" />
```

# Code Example

```csharp

/**
*
* Because an error occurs inside the transaction scope, the creation of the file will not take place
* (or better said be rolled back):
*
* txFileSystem.File.Exists(@"C:\Users\JohnDoe\Documents\example.txt");
*
* Would simply return 'false' after execution of the below code.
*
**/

using System.IO.Abstractions;
using System.Transactions;
using EQXMedia.TxFileSystem;
    
using var transactionScope = TransactionScope();

var fileSystem = new FileSystem();
var txFileSystem = new TxFileSystem(fileSystem);
txFileSystem.File.Create(@"C:\Users\JohnDoe\Documents\example.txt");

throw new Exception("Error occurs after creating the file. Resulting in the creation to be rolled back.");

transactionScope.Complete();

```
