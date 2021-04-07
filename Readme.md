TxFileSystem
=================
`TxFileSystem` is a transactional filesystem wrapper using the .NET Core filesystem abstraction from `System.IO.Abstractions`.

This filesystem wrapper supports transactional operations on:
*   Files,
*   Directories,
*   Filestreams.

Code Example
-------------

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

using var transactionScope = TransactionScope();

var fileSystem = new FileSystem();
var txFileSystem = new TxFileSystem(fileSystem);
txFileSystem.File.Create(@"C:\Users\JohnDoe\Documents\example.txt");

throw new Exception("Error occurs after creating the file. Resulting in the creation to be rolled back.");

transactionScope.Complete();

```
