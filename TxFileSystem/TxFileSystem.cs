namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional filesystem is actually a wrapper around filesystems that implement the interfaces from
    ///   <see href="https://www.nuget.org/packages/System.IO.Abstractions/">System.IO.Abstractions</see>.
    ///   
    ///   <para>File, directory and filestream operations performed on this transactional filesystem, will be rolled back
    ///   whenever an exception occurs inside the transaction scope.</para>
    ///   
    ///   <para>All operations that make modifications to files, directories or files in created filestreams, result in
    ///   backups to be created, they are then journalized by the transactional filesystem.</para>
    ///   
    ///   <para>When an error occurs inside the transaction scope, all operations inside the journal are rolled back. 
    ///   Causing the backups of the files and directories to be restored. If the transaction scope completes successfully, 
    ///   all operations are committed. Resulting in the backups being removed.</para>
    ///   
    ///   <para>This way data integrity is maintained when dealing with files and directories.</para>
    /// </summary>  
    /// <seealso cref="System.Transactions.TransactionScope"/>
    [Serializable]
    public sealed class TxFileSystem : IFileSystem
    {
        /// <summary>
        ///   Creates a <see cref="TxFileSystem"/>.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Always ensure that you create an instance of <see cref="TxFileSystem"/> inside the 
        ///     <see cref="System.Transactions.TransactionScope"/> to acivate the operations journal. Without doing so,
        ///     operations on files and directories won't be restored. Even when you perform them using the 
        ///     <see cref="TxFileSystem"/> instance created outside the scope.
        ///   </para>
        ///   <para>
        ///     Also, when you peform operations on the underlying filesystem passed to the constructor, you might loose 
        ///     data integrity, as those operations can't be rolled back when necessary.
        /// </para>
        /// </remarks>
        /// <seealso cref="System.Transactions.TransactionScope"/>
        /// <param name="fileSystem">The filesystem on which transactional operations should be performed.</param>
        public TxFileSystem(IFileSystem fileSystem = null)
        {
            this.FileSystem = fileSystem ?? new FileSystem();

            this.File = new TxFile(this);
            this.Directory = new TxDirectory(this);
            this.FileInfo = new TxFileInfo(this);
            this.FileStream = new TxFileStream(this);
            this.Path = new TxPath(this);
            this.DirectoryInfo = new TxDirectoryInfo(this);
            this.DriveInfo = new TxDriveInfo(this);
            this.FileSystemWatcher = new TxFileSystemWatcher(this);
        }

        /// <summary>
        ///   Provides operations for the creation, copying, deletion, moving, and opening of a single file,
        ///   and aids in the creation of <see cref="FileStream" /> objects.
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@type="FileOperation"]/*' />
        public IFile File { get; }

        ///<inheritdoc cref="IFileSystem.Directory"/>
        public IDirectory Directory { get; }

        ///<inheritdoc cref="IFileSystem.FileInfo"/>
        public IFileInfoFactory FileInfo { get; }

        ///<inheritdoc cref="IFileSystem.FileStream"/>
        public IFileStreamFactory FileStream { get; }

        internal IFileSystem FileSystem { get; set; }

        internal TxJournal Journal { get; } = new TxJournal();

        ///<inheritdoc cref="IFileSystem.Path"/>
        public IPath Path { get; }

        ///<inheritdoc cref="IFileSystem.DirectoryInfo"/>
        public IDirectoryInfoFactory DirectoryInfo { get; }

        ///<inheritdoc cref="IFileSystem.DriveInfo"/>
        public IDriveInfoFactory DriveInfo { get; }

        ///<inheritdoc cref="IFileSystem.FileSystemWatcher"/>
        public IFileSystemWatcherFactory FileSystemWatcher { get; }
    }
}
