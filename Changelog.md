TxFileSystem
=================
`TxFileSystem` is a transactional filesystem wrapper using the .NET filesystem abstraction from `System.IO.Abstractions`.

## Version 2.0.2

### Fixes
* Directories already existing when invoking `Directory.CreateDirectory` are no longer removed after rollback execution.

## Version 2.0.1

### Improvements
* Added support for Symbol files (`.pdb`).
* Added testing helpers to aid projects in Unit Testing.

## Version 2.0.0

### Fixes
* `TxFileSystem` class is now a `IFileSystem` implementation too.
* Exposing the `TxFileSystem` on property instances too to maintain data integrity.

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

*  Providing a `System.IO.Abstractions` filesystem wrapper.
*  Can be used to wrap any filesystem that implements the `IFileSystem` interface.
*  Support transactional operations on:
   *   Files,
   *   Directories,
   *   Filestreams.
*  Plain proxy functionality for all remaining parts of the `System.IO.Abstractions` interfaces.
*  Fully covered by Unit Tests (**100% code coverage**).
