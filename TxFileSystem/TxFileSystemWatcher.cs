namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional file system watcher exposes a factory to aid in the creation of <see 
    ///     cref="System.IO.FileSystemWatcher" /> objects. Which enables listening to file system change 
    ///   notifications and raise events when a directory, or file in a directory, changes.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxFileSystemWatcher</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystemWatcher" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
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