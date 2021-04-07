namespace EQXMedia.TxFileSystem.Abstractions
{
    using System.IO.Abstractions;

    internal interface ITxFileSystemWatcher : IFileSystemWatcherFactory
    {
        public IFileSystem FileSystem { get; }

        internal ITxFileSystem TxFileSystem { get; set; }
    }
}