#region Constructor_ExampleOne

using System.IO.Abstractions;
using EQXMedia.TxFileSystem;

var txFileSystem = new TxFileSystem(new FileSystem());

#endregion

#region Constructor_ExampleTwo

using EQXMedia.TxFileSystem;

var txFileSystem = new TxFileSystem();

#endregion

#region Constructor_ExampleThree

using System.IO.Abstractions.TestingHelpers;
using EQXMedia.TxFileSystem;

var mockFileSystem = new MockFileSystem();
var txFileSystem = new TxFileSystem(mockFileSystem);

#endregion