﻿namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional file system watcher exposes a factory to aid in the creation of <see 
    ///     cref="System.IO.FileSystemWatcher" /> objects. Which enables listening to file system change 
    ///   notifications and raise events when a directory, or file in a directory, changes.
    /// </summary>
    /// <remarks>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    ///   <note type="note">
    ///     <c>TxFileSystemWatcher</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystemWatcher" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </note>
    /// </remarks>
    [Serializable]
    public sealed class TxFileSystemWatcher : IFileSystemWatcherFactory
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxFileSystemWatcher(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        /// <inheritdoc cref="System.IO.FileSystemWatcher()" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@type="TxFileSystemWatcher" and @method="CreateNew"]/*' />
        public IFileSystemWatcher CreateNew()
        {
            return this.TxFileSystem.FileSystem.FileSystemWatcher.CreateNew();
        }

        /// <inheritdoc cref="System.IO.FileSystemWatcher(string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@type="TxFileSystemWatcher" and @method="CreateNew"]/*' />
        public IFileSystemWatcher CreateNew(string path)
        {
            return this.TxFileSystem.FileSystem.FileSystemWatcher.CreateNew(path);
        }

        /// <inheritdoc cref="System.IO.FileSystemWatcher(string, string)" />
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@type="TxFileSystemWatcher" and @method="CreateNew"]/*' />
        public IFileSystemWatcher CreateNew(string path, string filter)
        {
            return this.TxFileSystem.FileSystem.FileSystemWatcher.CreateNew(path, filter);
        }
    }
}