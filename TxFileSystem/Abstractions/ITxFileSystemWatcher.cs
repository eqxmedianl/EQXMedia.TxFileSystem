namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    internal interface ITxFileSystemWatcher : IFileSystemWatcherFactory
    {
        IFileSystem FileSystem { get; }
    }
}