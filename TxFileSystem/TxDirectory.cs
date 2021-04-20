namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Operations.Directories;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Security.AccessControl;

#if NET5_0
    using System.Runtime.Versioning;
#endif

    /// <remarks>
    ///   <c>TxDirectory</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.Directory" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
    public sealed class TxDirectory : IDirectory
    {
        internal TxDirectory(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" />, which actually is an 
        ///   implementation of <see cref="System.IO.Abstractions.IFileSystem" /> itself too.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This property is exposed by the <see cref="System.IO.Abstractions.IDirectory" /> interface. The 
        ///     way it is implemented in this library, ensures that all operations performed through this 
        ///     property, are transactional too. Whenever required.
        ///   </para>
        /// </remarks>
        public IFileSystem FileSystem => this.TxFileSystem;

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> this <see 
        ///     cref="EQXMedia.TxFileSystem.TxDirectory" /> instance belongs to. Thus not the actual file system 
        ///     being wrapped.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///      Use <see cref="EQXMedia.TxFileSystem.TxFileSystem.FileSystem" /> to perform operations on the 
        ///      wrapped file system.
        ///   </para>
        /// </remarks>
        internal TxFileSystem TxFileSystem { get; set; }

        /// <inheritdoc cref="System.IO.Directory.CreateDirectory(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="create"]/*' />
        public IDirectoryInfo CreateDirectory(string path)
        {
            return new CreateDirectoryOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.CreateDirectory(string)"/>
        /// <param name="path">The directory to create.</param>
        /// <param name="directorySecurity">The permissions that should be applied to created directory.</param>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="create"]/*' />
        public IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return new CreateDirectoryOperation(this, path, directorySecurity).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.Delete(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="delete"]/*' />
        public void Delete(string path)
        {
            new DeleteOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.Delete(string, bool)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="delete"]/*' />
        public void Delete(string path, bool recursive)
        {
            new DeleteOperation(this, path, recursive).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateDirectories(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return new EnumerateDirectoriesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateDirectories(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return new EnumerateDirectoriesOperation(this, path, searchPattern).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateDirectories(string, string, SearchOption)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern,
            SearchOption searchOption)
        {
            return new EnumerateDirectoriesOperation(this, path, searchPattern, searchOption).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return new EnumerateDirectoriesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }
#endif

        /// <inheritdoc cref="System.IO.Directory.EnumerateFiles(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateFiles(string path)
        {
            return new EnumerateFilesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateFiles(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return new EnumerateFilesOperation(this, path, searchPattern).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateFiles(string, string, SearchOption)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return new EnumerateFilesOperation(this, path, searchPattern, searchOption).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        public IEnumerable<string> EnumerateFiles(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return new EnumerateFilesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }
#endif

        /// <inheritdoc cref="System.IO.Directory.EnumerateFileSystemEntries(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return new EnumerateFileSystemEntriesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateFileSystemEntries(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return new EnumerateFileSystemEntriesOperation(this, path, searchPattern).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.EnumerateFileSystemEntries(string, string, SearchOption)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern,
            SearchOption searchOption)
        {
            return new EnumerateFileSystemEntriesOperation(this, path, searchPattern, searchOption).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return new EnumerateFileSystemEntriesOperation(this, path, searchPattern, enumerationOptions)
                .Execute();
        }
#endif

        /// <inheritdoc cref="System.IO.Directory.Exists(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public bool Exists(string path)
        {
            return new ExistsOperation(this, path).Execute();
        }

#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public DirectorySecurity GetAccessControl(string path)
        {
            return new GetAccessControlOperation(this, path).Execute();
        }

#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return new GetAccessControlOperation(this, path, includeSections).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetCreationTime(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public DateTime GetCreationTime(string path)
        {
            return new GetCreationTimeOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetCreationTimeUtc(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public DateTime GetCreationTimeUtc(string path)
        {
            return new GetCreationTimeOperation(this, path, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetCurrentDirectory()"/>
        public string GetCurrentDirectory()
        {
            return new GetCurrentDirectoryOperation(this).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetDirectories(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetDirectories(string path)
        {
            return new GetDirectoriesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetDirectories(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetDirectories(string path, string searchPattern)
        {
            return new GetDirectoriesOperation(this, path, searchPattern).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetDirectories(string, string, SearchOption)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return new GetDirectoriesOperation(this, path, searchPattern, searchOption).Execute();
        }


#if !NETSTANDARD2_0 && !NET461
        public string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return new GetDirectoriesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }
#endif

        /// <inheritdoc cref="System.IO.Directory.GetDirectoryRoot(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string GetDirectoryRoot(string path)
        {
            return new GetDirectoryRootOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetFiles(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetFiles(string path)
        {
            return new GetFilesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetFiles(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetFiles(string path, string searchPattern)
        {
            return new GetFilesOperation(this, path, searchPattern).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetFiles(string, string, SearchOption)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return new GetFilesOperation(this, path, searchPattern, searchOption).Execute();
        }

#if !NETSTANDARD2_0 && !NET461
        public string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return new GetFilesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }
#endif

        /// <inheritdoc cref="System.IO.Directory.GetFileSystemEntries(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetFileSystemEntries(string path)
        {
            return new GetFileSystemEntriesOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetFileSystemEntries(string, string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return new GetFileSystemEntriesOperation(this, path, searchPattern).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetLastAccessTime(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public DateTime GetLastAccessTime(string path)
        {
            return new GetLastAccessTimeOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetLastAccessTimeUtc(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public DateTime GetLastAccessTimeUtc(string path)
        {
            return new GetLastAccessTimeOperation(this, path, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetLastWriteTime(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public DateTime GetLastWriteTime(string path)
        {
            return new GetLastWriteTimeOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetLastWriteTimeUtc(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public DateTime GetLastWriteTimeUtc(string path)
        {
            return new GetLastWriteTimeOperation(this, path, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetLogicalDrives()"/>
        public string[] GetLogicalDrives()
        {
            return new GetLogicalDrivesOperation(this).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.GetParent(string)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="info"]/*' />
        public IDirectoryInfo GetParent(string path)
        {
            return new GetParentOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.Move(string, string)"/>
        public void Move(string sourceDirName, string destDirName)
        {
            new MoveOperation(this, sourceDirName, destDirName).Execute();
        }

#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            new SetAccessControlOperation(this, path, directorySecurity).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetCreationTime(string, DateTime)"/>
        public void SetCreationTime(string path, DateTime creationTime)
        {
            new SetCreationTimeOperation(this, path, creationTime).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetCreationTimeUtc(string, DateTime)"/>
        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            new SetCreationTimeOperation(this, path, creationTimeUtc, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetCurrentDirectory(string)"/>
        public void SetCurrentDirectory(string path)
        {
            new SetCurrentDirectoryOperation(this, path).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastAccessTime(string, DateTime)"/>
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTime).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastAccessTimeUtc(string, DateTime)"/>
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTimeUtc, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastWriteTime(string, DateTime)"/>
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTime).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastWriteTimeUtc(string, DateTime)"/>
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTimeUtc, asUtc: true).Execute();
        }
    }
}
