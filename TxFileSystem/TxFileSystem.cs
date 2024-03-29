﻿namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Exceptions;
    using global::EQXMedia.TxFileSystem.Journaling;
    using System;
    using System.IO.Abstractions;
    using System.Runtime.Serialization;

    /// <summary>
    ///   Transactional file system is actually a wrapper around file systems that implement the 
    ///   <c>System.IO.Abstractions.IFileSystem</c> interface. Proving transactional operations on files,
    ///   directories and file streams using the file system it wraps.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     File, directory and file stream operations performed through <see cref="EQXMedia.TxFileSystem.TxFileSystem" />,
    ///     will be rolled back whenever an exception occurs inside the transaction scope, if they altered a file 
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
    ///   
    ///   <para>
    ///     By creating the instance of <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> inside the
    ///     <see cref="System.Transactions.TransactionScope" />, the journal providing the rollback
    ///     functionality becomes active, resulting in data integrity to be preserved.
    ///   </para>
    ///
    ///   <para>
    ///     Creating the instance of <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> outside the <see 
    ///     cref="System.Transactions.TransactionScope" />, results in the journal providing the rollback
    ///     functionality not being used. In this case the<see cref="EQXMedia.TxFileSystem.TxFileSystem" />
    ///     instance will function as the regular <c>System.IO.Abstractions.IFileSystem</c>
    ///     implementation instance it wraps.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://txfilesystem.io/docs/TxFileSystem" />
    /// <seealso href="https://github.com/System-IO-Abstractions/System.IO.Abstractions" 
    ///   alt="System.IO.Abstractions on NuGet"/>
    /// <seealso href="https://www.nuget.org/packages/System.IO.Abstractions/" 
    ///   alt="System.IO.Abstractions on GitHub"/>
    [Serializable]
    public sealed class TxFileSystem : IFileSystem, ISerializable
    {
        /// <summary>
        ///   Creates a <see cref="TxFileSystem"/> instance.
        ///   
        ///   When no file system is passed using <paramref name="fileSystem" /> an instance of 
        ///   <c>System.IO.Abstractions.FileSystem</c> is used instead.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Always ensure that you create an instance of <see cref="TxFileSystem"/> inside the 
        ///     <see cref="System.Transactions.TransactionScope"/> to activate the transactional journal. 
        ///     Without doing so, operations on files and directories are not transactional. Having 
        ///     created a <see cref="TxFileSystem" /> instance outside a <see 
        ///       cref="System.Transactions.TransactionScope"/> and still performing the operations using 
        ///     it inside the scope, the operations are simply not transactional.
        ///   </para>
        ///   <note type="caution">
        ///     <conceptualLink target="Transactional_Journal#RollbackAndCommit">Backup and/or 
        ///       restore</conceptualLink> only take place for transactional operations journalized 
        ///     by an active <conceptualLink 
        ///       target="Transactional_Journal#JournalActivation">Transactional Journal
        ///     </conceptualLink>. Ensure that it <conceptualLink 
        ///       target="Transactional_Journal#JournalActivation"> gets activated</conceptualLink>.
        ///   </note>
        ///   <para>
        ///     Also, when you perform operations directory a file system you pass to the constructor, 
        ///     you loose data integrity in case of exceptions. Because doing that the journal providing 
        ///     the rollback functionality will simply not be used.
        /// </para>
        /// </remarks>
        /// <example>
        /// The first sample shows how to create an instance by passing it the default file system 
        /// implementation. Resulting in that implementation to be used internally.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Constructor_ExampleOne" lang="C#" 
        ///   title="Create an instance using specified file system implementation" />
        /// 
        /// The second sample shows how to create an instance without passing it a file system 
        /// implementation. Resulting in the default implementation to be used internally.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Constructor_ExampleTwo" lang="C#" 
        ///   title="Create an instance using the default file system implementation" />
        /// 
        /// The third sample shows how to create an instance by passing it a mock file system 
        /// implementation. This way the application logic can be Unit Tested without touching the 
        /// actual file system.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Constructor_ExampleThree" lang="C#" 
        ///   title="Create an instance using a mock file system implementation" />
        /// 
        /// The third sample shows how to create an instance inside a transaction scope, enabling support 
        /// for transactional operations. This is the only way to activate the journal and make use of 
        /// the backup and rollback functionality TxFileSystem provides automatically.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Constructor_ExampleFour" lang="C#" 
        ///   title="Create an instance inside a transaction scope, enabling support for transactional operations" />
        ///   
        /// </example>
        /// <param name="fileSystem">
        ///   A file system on which transactional operations should be performed. The default value is <c>null</c>.
        /// </param>
        /// <exception cref="UnsupportedFileSystemImplementationException">This exception is thrown when the specified <c>fileSytem</c> is actually a <c>TxFileSystem</c> itself too.</exception>
        /// <seealso cref="System.Transactions.TransactionScope"/>
        /// <seealso href="https://github.com/System-IO-Abstractions/System.IO.Abstractions" 
        ///   alt="System.IO.Abstractions on GitHub"/>
        /// <seealso href="https://www.nuget.org/packages/System.IO.Abstractions/" 
        ///   alt="System.IO.Abstractions on NuGet"/>
        public TxFileSystem(IFileSystem fileSystem = null)
        {
            if (fileSystem is TxFileSystem)
            {
                throw new UnsupportedFileSystemImplementationException(fileSystem);
            }

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
        ///   This constructor exists to ensure that deserialization of this class happens without 
        ///   any exceptions.
        /// </summary>
        /// <seealso cref="EQXMedia.TxFileSystem.TxFileSystem.GetObjectData(SerializationInfo, StreamingContext)" />
        private TxFileSystem(SerializationInfo info, StreamingContext context)
        {
            var tmpFileSytem = (IFileSystem)info.GetValue("fileSystem", typeof(IFileSystem));

            if (tmpFileSytem is TxFileSystem)
            {
                this.FileSystem = new FileSystem();
            }
            else
            {
                this.FileSystem = tmpFileSytem;
            }

            this.File = new TxFile(this);
            this.Directory = new TxDirectory(this);
            this.FileInfo = new TxFileInfo(this);
            this.FileStream = new TxFileStream(this);
            this.Path = new TxPath(this);
            this.DirectoryInfo = new TxDirectoryInfo(this);
            this.DriveInfo = new TxDriveInfo(this);
            this.FileSystemWatcher = new TxFileSystemWatcher(this);
        }

        /// <exclude />
        /// <seealso cref="EQXMedia.TxFileSystem.TxFileSystem(SerializationInfo, StreamingContext)" />
        /// <remarks>
        ///   Exists in order for the deserialization constructor to function properly.
        /// </remarks>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fileSystem", this.FileSystem);
        }

        /// <summary>
        ///   Exposes methods for the creation, copying, deletion, moving, and opening of a single file,
        ///   and aids in the creation of <see cref="System.IO.FileStream" /> objects.
        /// </summary>
        /// <example>
        /// The first sample shows a method being used that creates a file. Thus, underlying the journal 
        /// is used providing rollback functionality in case of exceptions.
        /// 
        /// If the transaction scope completes successfully the created file is kept, otherwise the 
        /// file will be removed upon rolling back the journalized operations.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Property_File_ExampleOne" lang="C#" 
        ///   title="Using a transactional operation inside a transaction scope" />
        /// 
        /// The second sample shows both a journalizing operation being used and one that is not 
        /// journalized.
        /// 
        /// Only creating the file will be rolled back if an exception occurs. The check to
        /// see whether the file exists or not doesn't have to be rolled back. Simply because 
        /// it doesn't modify any file system resources.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Property_File_ExampleTwo" lang="C#" 
        ///   title="Using both a transactional operation and a non-transactional operation" />
        ///   
        /// </example>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="true"]/*' />
        public TxFile File { get; }

        /// <summary>
        ///   Exposes methods for creating, moving, and enumerating through directories and subdirectories.
        /// </summary>
        /// <example>
        /// The first sample shows a method being used that creates a directory. Thus, underlying the journal 
        /// is used providing rollback functionality in case of exceptions.
        /// 
        /// If the transaction scope completes successfully the created directory is kept, otherwise the 
        /// directory will be removed upon rolling back the journalized operations.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Property_Directory_ExampleOne" lang="C#" 
        ///   title="Using a transactional operation inside a transaction scope" />
        /// 
        /// The second sample shows both a journalizing operation being used and one that is not 
        /// journalized.
        /// 
        /// Only creating the directory will be rolled back if an exception occurs. The check to
        /// see whether the directory exists or not doesn't have to be rolled back. Simply 
        /// because it doesn't modify any file system resources.
        ///     
        /// <code source="..\Examples\TxFileSystem_Examples.cs" region="Property_Directory_ExampleOne" lang="C#" 
        ///   title="Using both a transactional operation and a non-transactional operation" />
        ///   
        /// </example>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="true"]/*' />
        public TxDirectory Directory { get; }

        /// <summary>
        ///   <para>
        ///     Exposes methods to aid in the creation of <see cref="System.IO.FileInfo" /> objects.
        ///     Which can be used to get information about files.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxFileInfo FileInfo { get; }

        /// <summary>
        ///   <para>
        ///     Exposes a factory to create a <see cref="System.IO.Stream" /> for a file, supporting 
        ///     both synchronous and asynchronous read and write operations.
        ///   </para>
        /// </summary>        
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="true"]/*' />
        public TxFileStream FileStream { get; }

        /// <summary>
        ///   The actual file system being wrapped which is used to perform the operations on.
        /// </summary>
        /// <remarks>
        ///   This is only to be used internally by this library.
        /// </remarks>
        internal IFileSystem FileSystem { get; set; }

        /// <summary>
        ///   The journal which stores the transactional operations that might have to be rolled back.
        /// </summary>
        /// <remarks>
        ///   This is only to be used internally by this library.
        /// </remarks>
        internal TxJournal Journal { get; } = new TxJournal();

        /// <summary>
        ///   <para>
        ///     Exposes methods to perform operations on <see cref="System.String"/> instances that 
        ///     contain file or directory path information. These operations are performed in a 
        ///     cross-platform manner.
        ///   </para>
        /// </summary>
        public TxPath Path { get; }

        /// <summary>
        ///   <para>
        ///     Exposes methods to aid in the creation of <see cref="System.IO.DirectoryInfo" /> 
        ///     objects. Which can be used to get information about directories.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxDirectoryInfo DirectoryInfo { get; }

        /// <summary>
        ///   <para>
        ///     Exposes methods to aid in the creation of <see cref="System.IO.DriveInfo" /> 
        ///     objects. Which can be used to get information about drives.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxDriveInfo DriveInfo { get; }

        /// <summary>
        ///   <para>
        ///     Exposes a factory to aid in the creation of <see cref="System.IO.FileSystemWatcher" /> 
        ///     objects. Which enables listening to file system change notifications and raise events 
        ///     when a directory, or file in a directory, changes.
        ///   </para>
        /// </summary>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/FileSystemProperties/FileSystemProperty[@modifying="false"]/*' />
        public TxFileSystemWatcher FileSystemWatcher { get; }

        IFile IFileSystem.File => this.File;

        IDirectory IFileSystem.Directory => this.Directory;

        IFileInfoFactory IFileSystem.FileInfo => this.FileInfo;

        IFileStreamFactory IFileSystem.FileStream => this.FileStream;

        IPath IFileSystem.Path => this.Path;

        IDirectoryInfoFactory IFileSystem.DirectoryInfo => this.DirectoryInfo;

        IDriveInfoFactory IFileSystem.DriveInfo => this.DriveInfo;

        IFileSystemWatcherFactory IFileSystem.FileSystemWatcher => this.FileSystemWatcher;
    }
}
