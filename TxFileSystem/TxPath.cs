namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions;

    /// <remarks>
    ///   <c>TxPath</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.Path" />. 
    ///   It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    /// </remarks>
    [Serializable]
    public sealed class TxPath : IPath
    {
        internal TxPath(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <summary>
        ///   Returns the <see cref="EQXMedia.TxFileSystem.TxFileSystem" />, which actually is an 
        ///   implementation of <c>System.IO.Abstractions.IFileSystem</c> itself too.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This property is exposed by the <c>System.IO.Abstractions.IPath</c> interface. The way it is 
        ///     implemented in this library, ensures that all operations performed through this property, are 
        ///     transactional too. Whenever required.
        ///   </para>
        ///   <para>
        ///     This is useful for implementing extension methods.
        ///   </para>
        /// </remarks>
        public IFileSystem FileSystem => this.TxFileSystem;

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

        public char AltDirectorySeparatorChar => this.TxFileSystem.FileSystem.Path.AltDirectorySeparatorChar;

        public char DirectorySeparatorChar => this.TxFileSystem.FileSystem.Path.DirectorySeparatorChar;

        public char PathSeparator => this.TxFileSystem.FileSystem.Path.PathSeparator;

        public char VolumeSeparatorChar => this.TxFileSystem.FileSystem.Path.VolumeSeparatorChar;

        public char[] InvalidPathChars => this.TxFileSystem.FileSystem.Path.InvalidPathChars;

        public string ChangeExtension(string path, string extension)
        {
            return this.TxFileSystem.FileSystem.Path.ChangeExtension(path, extension);
        }

        public string Combine(params string[] paths)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(paths);
        }

        public string Combine(string path1, string path2)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(path1, path2);
        }

        public string Combine(string path1, string path2, string path3)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(path1, path2, path3);
        }

        public string Combine(string path1, string path2, string path3, string path4)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(path1, path2, path3, path4);
        }

        public string GetDirectoryName(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetDirectoryName(path);
        }

        public string GetExtension(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetExtension(path);
        }

        public string GetFileName(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFileName(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFileNameWithoutExtension(path);
        }

        public string GetFullPath(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFullPath(path);
        }

#if !NETSTANDARD2_0 && !NET461
        public string GetFullPath(string path, string basePath)
        {
            return this.TxFileSystem.FileSystem.Path.GetFullPath(path, basePath);
        }
#endif

        public char[] GetInvalidFileNameChars()
        {
            return this.TxFileSystem.FileSystem.Path.GetInvalidFileNameChars();
        }

        public char[] GetInvalidPathChars()
        {
            return this.TxFileSystem.FileSystem.Path.GetInvalidPathChars();
        }

        public string GetPathRoot(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetPathRoot(path);
        }

        public string GetRandomFileName()
        {
            return this.TxFileSystem.FileSystem.Path.GetRandomFileName();
        }

#if !NETSTANDARD2_0 && !NET461
        public string GetRelativePath(string relativeTo, string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetRelativePath(relativeTo, path);
        }
#endif

        public string GetTempFileName()
        {
            return this.TxFileSystem.FileSystem.Path.GetTempFileName();
        }

        public string GetTempPath()
        {
            return this.TxFileSystem.FileSystem.Path.GetTempPath();
        }

        public bool HasExtension(string path)
        {
            return this.TxFileSystem.FileSystem.Path.HasExtension(path);
        }

#if !NETSTANDARD2_0 && !NET461
        public bool IsPathFullyQualified(string path)
        {
            return this.TxFileSystem.FileSystem.Path.IsPathFullyQualified(path);
        }
#endif

        public bool IsPathRooted(string path)
        {
            return this.TxFileSystem.FileSystem.Path.IsPathRooted(path);
        }

#if !NETSTANDARD2_0 && !NET461
        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2);
        }

        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2, path3);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3,
            Span<char> destination, out int charsWritten)
        {
            return this.TxFileSystem.FileSystem.Path.TryJoin(path1, path2, path3, destination, out charsWritten);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination,
            out int charsWritten)
        {
            return this.TxFileSystem.FileSystem.Path.TryJoin(path1, path2, destination, out charsWritten);
        }
#endif
    }
}