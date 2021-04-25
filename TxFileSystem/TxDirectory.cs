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

    /// <summary>
    ///   Transactional directory exposes methods for creating, moving, enumerating through directories 
    ///   and subdirectories and so on.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxDirectory</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.Directory" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="true"]/remarks/*' />
    /// </remarks>
    [Serializable]
    public sealed class TxDirectory : IDirectory
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxDirectory(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="FileSystem"]/*' />
        public IFileSystem FileSystem => this.TxFileSystem;

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
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

#if ENUMERATING_IO
        /// <inheritdoc cref="System.IO.Directory.EnumerateDirectories(string, string, EnumerationOptions)" />
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

#if ENUMERATING_IO
        /// <inheritdoc cref="System.IO.Directory.EnumerateFiles(string, string, EnumerationOptions)" />
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

#if ENUMERATING_IO
        /// <inheritdoc cref="System.IO.Directory.EnumerateFileSystemEntries(string, string, EnumerationOptions)" />
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

        /// <summary>
        ///   Returns the Windows access control list (ACL) for a directory.
        /// </summary>
        /// <param name="path">The path to a directory containing a <see cref="System.Security.AccessControl.DirectorySecurity">DirectorySecurity</see> object that describes the file's access control list (ACL) information.</param>
        /// <returns>An object that encapsulates the access control rules for the file described by the <c>path</c> parameter.</returns>
        /// <remarks>
        ///   <para>
        ///     Use the GetAccessControl method to retrieve the access control list (ACL) entries for a directory.
        ///   </para>
        ///   
        ///   <para>
        ///     An ACL describes individuals and/or groups who have, or do not have, rights to specific actions on the 
        ///     given file or directory.For more information, see How to: Add or Remove Access Control List Entries.
        ///   </para>
        ///   
        ///   <para>
        ///     In NTFS environments, ReadAttributes and ReadExtendedAttributes are granted to the user if the user has 
        ///     ListDirectory rights on the parent folder. To deny ReadAttributes and ReadExtendedAttributes, deny 
        ///     ListDirectory on the parent directory.
        ///   </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <c>path</c> parameter is <c>null</c>.</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the directory.</exception>
        /// <exception cref="SystemException">A system-level error occurred, such as the directory could not be found. 
        ///   The specific exception may be a subclass of <see cref="SystemException" />.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///   <para>The path parameter specified a directory that is read-only.</para>
        ///   <para>-or-</para>
        ///   <para>This operation is not supported on the current platform.</para>
        ///   <para>-or-</para>
        ///   <para>The caller does not have the required permission.</para>
        /// </exception>
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.GetAccessControl" />
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public DirectorySecurity GetAccessControl(string path)
        {
            return new GetAccessControlOperation(this, path).Execute();
        }

        /// <summary>
        ///   Returns the Windows access control list (ACL) for a directory.
        /// </summary>
        /// <param name="path">The path to a directory containing a <see cref="System.Security.AccessControl.DirectorySecurity">DirectorySecurity</see> object that describes the file's access control list (ACL) information.</param>
        /// <param name="includeSections">One of the <see cref="System.Security.AccessControl.AccessControlSections">AccessControlSections</see> values that specifies the type of access control list (ACL) information to receive.</param>
        /// <returns>An object that encapsulates the access control rules for the file described by the <c>path</c> parameter.</returns>
        /// <remarks>
        ///   <para>
        ///     Use the GetAccessControl method to retrieve the access control list (ACL) entries for a directory.
        ///   </para>
        ///   
        ///   <para>
        ///     An ACL describes individuals and/or groups who have, or do not have, rights to specific actions on the 
        ///     given file or directory.For more information, see How to: Add or Remove Access Control List Entries.
        ///   </para>
        ///   
        ///   <para>
        ///     In NTFS environments, ReadAttributes and ReadExtendedAttributes are granted to the user if the user has 
        ///     ListDirectory rights on the parent folder. To deny ReadAttributes and ReadExtendedAttributes, deny 
        ///     ListDirectory on the parent directory.
        ///   </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <c>path</c> parameter is <c>null</c>.</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the directory.</exception>
        /// <exception cref="SystemException">A system-level error occurred, such as the directory could not be found. 
        ///   The specific exception may be a subclass of <see cref="SystemException" />.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///   <para>The path parameter specified a directory that is read-only.</para>
        ///   <para>-or-</para>
        ///   <para>This operation is not supported on the current platform.</para>
        ///   <para>-or-</para>
        ///   <para>The caller does not have the required permission.</para>
        /// </exception>
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.GetAccessControl" />
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
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileSystemOperation" and @type="info"]/*' />
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


#if ENUMERATING_IO
        /// <inheritdoc cref="System.IO.Directory.GetDirectories(string, string, EnumerationOptions)"/>
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

#if ENUMERATING_IO
        /// <inheritdoc cref="System.IO.Directory.GetFiles(string, string, EnumerationOptions)"/>
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
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="FileSystemOperation" and @type="info"]/*' />
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
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
        public void Move(string sourceDirName, string destDirName)
        {
            new MoveOperation(this, sourceDirName, destDirName).Execute();
        }

        /// <summary>
        ///   Applies access control list (ACL) entries described by a <see cref="DirectorySecurity">DirectorySecurity</see> object to the specified directory.
        /// </summary>
        /// <param name="path">A directory to add or remove access control list (ACL) entries from.</param>
        /// <param name="directorySecurity">A <see cref="DirectorySecurity">DirectorySecurity</see> object that describes an ACL entry to apply to the directory described by the <c>path</c> parameter.</param>
        /// <exception cref="ArgumentNullException">The <c>directorySecurity</c> parameter is <c>null</c>.</exception>
        /// <exception cref="DirectoryNotFoundException">The directory could not be found.</exception>
        /// <exception cref="ArgumentException">The <c>path</c> was invalid.</exception>
        /// <exception cref="UnauthorizedAccessException">
        ///   <para>The current process does not have access to the directory specified by <c>path</c>.</para>
        ///   <para>-or-</para>
        ///   <para>The current process does not have sufficient privilege to set the ACL entry.</para>
        /// </exception>
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.SetAccessControl" />
#if NET5_0
        [SupportedOSPlatform("windows")]
#endif
        public void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            new SetAccessControlOperation(this, path, directorySecurity).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetCreationTime(string, DateTime)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
        public void SetCreationTime(string path, DateTime creationTime)
        {
            new SetCreationTimeOperation(this, path, creationTime).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetCreationTimeUtc(string, DateTime)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
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
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTime).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastAccessTimeUtc(string, DateTime)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTimeUtc, asUtc: true).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastWriteTime(string, DateTime)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTime).Execute();
        }

        /// <inheritdoc cref="System.IO.Directory.SetLastWriteTimeUtc(string, DateTime)"/>
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Operations/Operation[@kind="DirectoryOperation" and @type="modify"]/*' />
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTimeUtc, asUtc: true).Execute();
        }
    }
}
