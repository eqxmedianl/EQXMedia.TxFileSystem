namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions;

    /// <remarks>
    ///   <c>TxFileSystemWatcher</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystemWatcher" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
    public sealed class TxFileSystemWatcher : ITxFileSystemWatcher
    {
        private readonly TxFileSystem _txFileSystem;

        internal TxFileSystemWatcher(TxFileSystem txFileSystem)
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