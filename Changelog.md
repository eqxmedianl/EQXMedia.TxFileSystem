TxFileSystem
=================
`TxFileSystem` is a transactional filesystem wrapper using the .NET Standard filesystem abstraction from `System.IO.Abstractions`.

Version 1.0.0
-----------------

*  Providing a `System.IO.Abstractions` filesystem wrapper.
*  Can be used to wrap any filesystem that implements the `IFileSystem` interface.
*  Support transactional operations on:
   *   Files,
   *   Directories,
   *   Filestreams.
*  Plain proxy functionality for all remaining parts of the `System.IO.Abstractions` interfaces.
*  Fully covered by Unit Tests (**100% code coverage**).
