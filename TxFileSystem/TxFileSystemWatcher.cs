namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions;

    internal sealed class TxFileSystemWatcher : ITxFileSystemWatcher
    {
        private readonly TxFileSystem _txFileSystem;

        public TxFileSystemWatcher(TxFileSystem txFileSystem)
        {
            _txFileSystem = txFileSystem;
        }

        public IFileSystem FileSystem => _txFileSystem.FileSystem;

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