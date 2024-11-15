# EQXMedia.TxFileSystem

`EQXMedia.TxFileSystem` is a transactional file system wrapper using the .NET file system abstraction from `System.IO.Abstractions`.

This file system wrapper supports transactional operations on:
*   Files,
*   Directories,
*   File streams.

## About the Project

### Build Status
[![.NET](https://github.com/eqxmedianl/EQXMedia.TxFileSystem/actions/workflows/dotnet.yml/badge.svg)](https://github.com/eqxmedianl/EQXMedia.TxFileSystem/actions/workflows/dotnet.yml)[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=eqxmedianl_EQXMedia.TxFileSystem&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=eqxmedianl_EQXMedia.TxFileSystem)

### Project Development

This library has been designed and implemented by [Jarno Kamminga](https://www.linkedin.com/in/jarnokamminga/ "Visit the profile of Jarno Kamminga on LinkedIn") for [EQX Media B.V.](https://www.eqx-media.nl "Visit the website of EQX Media B.V."), and published as an OpenSource project on GitHub.

### Project Website

The project has a website of its own which can be found at [https://txfilesystem.io/](https://txfilesystem.io/ "Visit the project website of EQXMedia.TxFileSystem").

### Library Documentation

The documentation of the library can be found at [https://txfilesystem.io/docs/](https://txfilesystem.io/docs/ "Read the documentation of EQXMedia.TxFileSystem").

## NuGet Package

A NuGet package is created of every `EQXMedia.TxFileSystem` release and can be installed to your .NET project using the *NuGet Package Manager* or `dotnet` command.

More information about the NuGet package can be found at NuGet.org:

[https://www.nuget.org/packages/EQXMedia.TxFileSystem/](https://www.nuget.org/packages/EQXMedia.TxFileSystem/ "Visit the NuGet package listing of EQXMedia.TxFileSystem")

### Installing the package

#### Package Manager
```powershell
PS> Install-Package EQXMedia.TxFileSystem -Version 3.0.0-beta1
```

#### .NET CLI
```
$ dotnet add package EQXMedia.TxFileSystem --version 3.0.0-beta1
```

#### Package Reference
```xml
<PackageReference Include="EQXMedia.TxFileSystem" Version="3.0.0-beta1" />
```

## Code Example

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

using System;
using System.IO.Abstractions;
using System.Transactions;
using EQXMedia.TxFileSystem;
    
using var transactionScope = TransactionScope();

var txFileSystem = new TxFileSystem(new FileSystem());
txFileSystem.File.Create(@"C:\Users\JohnDoe\Documents\example.txt");

throw new Exception("Error occurs after creating the file. Resulting in the creation to be rolled back.");

transactionScope.Complete();

```
