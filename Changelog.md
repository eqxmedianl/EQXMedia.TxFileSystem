# TxFileSystem
`TxFileSystem` is a transactional file system wrapper using the .NET file system abstraction from `System.IO.Abstractions`.

## Version 3.0.0-beta1

### Improvements
* Added support for .NET 6, .NET 7 and .NET 8.
* Added support for symbolic links.

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

### Features
* Added support for *IntelliSense*.
* Added documentation for all supported framework versions:
  * Generated **HTML Documentation**.
  * Generated **Windows Help File**.

### Improvements
* Minimized binary size of built libraries.

## Version 1.3.0

### Features

*  Added support for `.NET Framework 4.6.1`, as per `System.IO.Abstractions`.
*  Also Unit Testing this newly supported framework.

## Version 1.2.0

### Features

*  Added support for `.NET Standard 2.0`, as per `System.IO.Abstractions`.
*  Unit Testing all supported framework versions.

## Version 1.1.1

### Fixes

*  Bug in rollback functionality of `File.Move(string sourceFileName, string destFileName, bool overwrite)` (>= `.NET 5.0`) fixed.

## Version 1.1.0

### Features

*  Now supports multiple framework versions (currently being `.NET Standard 2.1` and `.NET 5.0`).

## Version 1.0.0

### Features

*  Providing a `System.IO.Abstractions` file system wrapper.
*  Can be used to wrap any file system that implements the `IFileSystem` interface.
*  Support transactional operations on:
   *   Files,
   *   Directories,
   *   File Streams.
*  Plain proxy functionality for all remaining parts of the `System.IO.Abstractions` interfaces.
*  Fully covered by Unit Tests (**100% code coverage**).
