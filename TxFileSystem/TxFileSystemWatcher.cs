namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal sealed class TxFileSystemWatcher : ITxFileSystemWatcher
    {
        public TxFileSystemWatcher(ITxFileSystem txFileSystem)
        {
            ((ITxFileSystemWatcher)this).TxFileSystem = txFileSystem;
        }

        ITxFileSystem ITxFileSystemWatcher.TxFileSystem { get; set; }

        public IFileSystem FileSystem => ((ITxFileSystemWatcher)this).TxFileSystem.FileSystem;

        public IFileSystemWatcher CreateNew()
        {
            return this.FileSystem.FileSystemWatcher.CreateNew();
        }

        public IFileSystemWatcher CreateNew(string path)
        {
            return this.FileSystem.FileSystemWatcher.CreateNew(path);

        }

        public IFileSystemWatcher CreateNew(string path, string filter)
        {
            return this.FileSystem.FileSystemWatcher.CreateNew(path, filter);
        }
    }
}