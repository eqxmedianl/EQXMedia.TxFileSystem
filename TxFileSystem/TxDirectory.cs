namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations.Directories;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Security.AccessControl;

#if NET5_0
    using System.Runtime.Versioning;
#endif

    public sealed class TxDirectory : ITxDirectory, IDirectory
    {
        public TxDirectory(ITxFileSystem txFileSystem)
        {
            ((ITxDirectory)this).TxFileSystem = txFileSystem;
        }

        public IFileSystem FileSystem => ((ITxDirectory)this).TxFileSystem.FileSystem;

        ITxFileSystem ITxDirectory.TxFileSystem { get; set; }

        public IDirectoryInfo CreateDirectory(string path)
        {
            return new CreateDirectoryOperation(this, path).Execute();
        }

        public IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return new CreateDirectoryOperation(this, path, directorySecurity).Execute();
        }

        public void Delete(string path)
        {
            new DeleteOperation(this, path).Execute();
        }

        public void Delete(string path, bool recursive)
        {
            new DeleteOperation(this, path, recursive).Execute();
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return new EnumerateDirectoriesOperation(this, path).Execute();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return new EnumerateDirectoriesOperation(this, path, searchPattern).Execute();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern,
            SearchOption searchOption)
        {
            return new EnumerateDirectoriesOperation(this, path, searchPattern, searchOption).Execute();
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return new EnumerateDirectoriesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return new EnumerateFilesOperation(this, path).Execute();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return new EnumerateFilesOperation(this, path, searchPattern).Execute();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return new EnumerateFilesOperation(this, path, searchPattern, searchOption).Execute();
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return new EnumerateFilesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return new EnumerateFileSystemEntriesOperation(this, path).Execute();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return new EnumerateFileSystemEntriesOperation(this, path, searchPattern).Execute();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern,
            SearchOption searchOption)
        {
            return new EnumerateFileSystemEntriesOperation(this, path, searchPattern, searchOption).Execute();
        }

        public IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return new EnumerateFileSystemEntriesOperation(this, path, searchPattern, enumerationOptions)
                .Execute();
        }

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

        public DateTime GetCreationTime(string path)
        {
            return new GetCreationTimeOperation(this, path).Execute();
        }

        public DateTime GetCreationTimeUtc(string path)
        {
            return new GetCreationTimeOperation(this, path, asUtc: true).Execute();
        }

        public string GetCurrentDirectory()
        {
            return new GetCurrentDirectoryOperation(this).Execute();
        }

        public string[] GetDirectories(string path)
        {
            return new GetDirectoriesOperation(this, path).Execute();
        }

        public string[] GetDirectories(string path, string searchPattern)
        {
            return new GetDirectoriesOperation(this, path, searchPattern).Execute();
        }

        public string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return new GetDirectoriesOperation(this, path, searchPattern, searchOption).Execute();
        }

        public string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return new GetDirectoriesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }

        public string GetDirectoryRoot(string path)
        {
            return new GetDirectoryRootOperation(this, path).Execute();
        }

        public string[] GetFiles(string path)
        {
            return new GetFilesOperation(this, path).Execute();
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return new GetFilesOperation(this, path, searchPattern).Execute();
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return new GetFilesOperation(this, path, searchPattern, searchOption).Execute();
        }

        public string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return new GetFilesOperation(this, path, searchPattern, enumerationOptions).Execute();
        }

        public string[] GetFileSystemEntries(string path)
        {
            return new GetFileSystemEntriesOperation(this, path).Execute();
        }

        public string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return new GetFileSystemEntriesOperation(this, path, searchPattern).Execute();
        }

        public DateTime GetLastAccessTime(string path)
        {
            return new GetLastAccessTimeOperation(this, path).Execute();
        }

        public DateTime GetLastAccessTimeUtc(string path)
        {
            return new GetLastAccessTimeOperation(this, path, asUtc: true).Execute();
        }

        public DateTime GetLastWriteTime(string path)
        {
            return new GetLastWriteTimeOperation(this, path).Execute();
        }

        public DateTime GetLastWriteTimeUtc(string path)
        {
            return new GetLastWriteTimeOperation(this, path, asUtc: true).Execute();
        }

        public string[] GetLogicalDrives()
        {
            return new GetLogicalDrivesOperation(this).Execute();
        }

        public IDirectoryInfo GetParent(string path)
        {
            return new GetParentOperation(this, path).Execute();
        }

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

        public void SetCreationTime(string path, DateTime creationTime)
        {
            new SetCreationTimeOperation(this, path, creationTime).Execute();
        }

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            new SetCreationTimeOperation(this, path, creationTimeUtc, asUtc: true).Execute();
        }

        public void SetCurrentDirectory(string path)
        {
            new SetCurrentDirectoryOperation(this, path).Execute();
        }

        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTime).Execute();
        }

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            new SetLastAccessTimeOperation(this, path, lastAccessTimeUtc, asUtc: true).Execute();
        }

        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTime).Execute();
        }

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            new SetLastWriteTimeOperation(this, path, lastWriteTimeUtc, asUtc: true).Execute();
        }
    }
}
