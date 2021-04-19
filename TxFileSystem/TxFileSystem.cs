namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional file system is actually a wrapper around file systems that implement the 
    ///   <see cref="System.IO.Abstractions.IFileSystem" /> interface.
    ///   
    ///   <para>
    ///     File, directory and file stream operations performed through <see cref="EQXMedia.TxFileSystem.TxFileSystem" />,
    ///     will be rolled back whenever an exception occurs inside the transaction scope, when they altered a file 
    ///     or directory.
    ///   </para>
    ///   
    ///   <para>
    ///     All operations that make modifications to files, directories or files in created file streams, result in
    ///     backups to be created. They are then journalized by <see cref="EQXMedia.TxFileSystem.TxFileSystem" />. 
    ///     Journalizing operations makes it possible for all of them to be rolled back when an error occurs.
    ///   </para>
    ///   
    ///   <para>
    ///     As said, when an error occurs inside the transaction scope, all operations inside the journal are rolled 
    ///     back. Causing the backups of the files and directories to be restored. On the other hand, if the transaction 
    ///     scope completes successfully, all operations that were journalized are committed. Resulting in the backups 
    ///     to be removed.
    ///   </para>
    ///   
    ///   <para>
    ///     The journaling ensures data integrity is maintained when exceptions occur after files and directories
    ///     have been modified.
    ///   </para>
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     By creating the instance of<see cref="EQXMedia.TxFileSystem.TxFileSystem" /> inside the
    ///     <see cref="System.Transactions.TransactionScope" />, the journal providing the rollback
    ///     functionality becomes active, resulting in data integrity to be preserved.
    ///   </para>
    ///
    ///   <para>
    ///     Creating the instance of <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> outside the
    ///     <see cref = "System.Transactions.TransactionScope" />, results in the journal providing the rollback
    ///     functionality not being used. In this case the<see cref="EQXMedia.TxFileSystem.TxFileSystem" />
    ///     instance will function as the regular <see cref = "System.IO.Abstractions.IFileSystem" />
    ///     implementation instance it wraps.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://txfilesystem.io/docs/TxFileSystem" />
    [Serializable]
    public sealed class TxFileSystem
    {
        /// <summary>
        ///   Creates a <see cref="TxFileSystem"/> instance.
        ///   
        ///   When no file system is passed using <paramref name="fileSystem" /> an instance of 
        ///   <see cref="System.IO.Abstractions.FileSystem" /> is used instead.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Always ensure that you create an instance of <see cref="TxFileSystem"/> inside the 
        ///     <see cref="System.Transactions.TransactionScope"/> to activate the operations journal. Without doing so,
        ///     operations on files and directories are not transactional. Having created a <see cref="TxFileSystem"/> 
        ///     instance outside a <see cref="System.Transactions.TransactionScope"/> and still performing the 
        ///     operations using it inside the scope, the operations are simply not transactional.
        ///   </para>
        ///   <para>
        ///     Also, when you perform operations directory a file system you pass to the constructor, you loose data 
        ///     integrity in case of exceptions. Because doing that the journal providing the rollback functionality 
        ///     will simply not be used.
        /// </para>
        /// </remarks>
        /// <param name="fileSystem">
        ///   A file system on which transactional operations should be performed (optional).
        /// </param>
        /// <seealso cref="System.Transactions.TransactionScope"/>
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
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="true"]/*' />
        public TxFile File { get; }

        /// <summary>
        ///   <para>
        ///     Exposes operations for creating, moving, and enumerating through directories and subdirectories.
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="true"]/*' />
        public TxDirectory Directory { get; }

        /// <summary>
        ///   <para>
        ///     Provides properties and methods for the creation, copying, deletion, moving, and opening files, 
        ///     and aids in the creation of <see cref="System.IO.FileStream" /> objects.
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxFileInfo FileInfo { get; }

        /// <summary>
        ///   <para>
        ///     Provides a <see cref="System.IO.Stream" /> for a file, supporting both synchronous and asynchronous 
        ///     read and write operations.
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>        
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="true"]/*' />
        public TxFileStream FileStream { get; }

        internal IFileSystem FileSystem { get; set; }

        internal TxJournal Journal { get; } = new TxJournal();

        /// <summary>
        ///   <para>
        ///     Performs operations on <see cref="System.String"/> instances that contain file or directory path 
        ///     information. These operations are performed in a cross-platform manner.
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>
        public TxPath Path { get; }

        /// <summary>
        ///   <para>
        ///     Exposes methods for creating, moving, and enumerating through directories and subdirectories. 
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxDirectoryInfo DirectoryInfo { get; }

        /// <summary>
        ///   <para>
        ///     Provides access to information on a drive.
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxDriveInfo DriveInfo { get; }

        /// <summary>
        ///   <para>
        ///     Listens to the file system change notifications and raises events when a directory, or file 
        ///     in a directory, changes.
        ///   </para>
        ///   <para>
        ///     This class cannot be inherited.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxFileSystemWatcher FileSystemWatcher { get; }
    }
}
