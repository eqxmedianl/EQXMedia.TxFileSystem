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
    public sealed class TxFileSystemWatcher : IFileSystemWatcherFactory
    {
        internal TxFileSystemWatcher(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> this <see 
        ///     cref="EQXMedia.TxFileSystem.TxFileSystemWatcher" /> instance belongs to. Thus not the actual 
        ///     file system being wrapped.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Use <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystem" /> to perform operations on 
        ///      the wrapped file system.
        ///   </para>
        /// </remarks>
        internal TxFileSystem TxFileSystem { get; set; }

        public IFileSystemWatcher CreateNew()
        {
            return this.TxFileSystem.FileSystem.FileSystemWatcher.CreateNew();
        }

        public IFileSystemWatcher CreateNew(string path)
        {
            return this.TxFileSystem.FileSystem.FileSystemWatcher.CreateNew(path);

        }

        public IFileSystemWatcher CreateNew(string path, string filter)
        {
            return this.TxFileSystem.FileSystem.FileSystemWatcher.CreateNew(path, filter);
        }
    }
}