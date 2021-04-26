TxFileSystem
=================
`TxFileSystem` is a transactional file system wrapper using the .NET file system abstraction from `System.IO.Abstractions`.

Version 2.0.0
-----------------

* `TxFileSystem` class is now a `IFileSystem` implementation too.
* Exposing the `TxFileSystem` on property instances too to maintain data integrity.
* Added documentation for all supported framework versions:
  * Generated **HTML Documentation**.
  * Generated **Windows Help File**.

Version 1.3.0
-----------------

*  Added support for `.NET Framework 4.6.1`, as per `System.IO.Abstractions`.
*  Also Unit Testing this newly supported framework.

Version 1.2.0
-----------------

*  Added support for `.NET Standard 2.0`, as per `System.IO.Abstractions`.
*  Unit Testing all supported framework versions.

Version 1.1.1
-----------------

*  Bug in rollback functionality of `File.Move(string sourceFileName, string destFileName, bool overwrite)` (>= `.NET 5.0`) fixed.

Version 1.1.0
-----------------

*  Now supports multiple framework versions (currently being `.NET Standard 2.1` and `.NET 5.0`).

Version 1.0.0
-----------------

*  Providing a `System.IO.Abstractions` file system wrapper.
*  Can be used to wrap any file system that implements the `IFileSystem` interface.
*  Support transactional operations on:
   *   Files,
   *   Directories,
   *   File Streams.
*  Plain proxy functionality for all remaining parts of the `System.IO.Abstractions` interfaces.
*  Fully covered by Unit Tests (**100% code coverage**).
