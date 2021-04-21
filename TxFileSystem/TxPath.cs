namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional path exposes methods to perform operations on <see cref="System.String"/> instances that 
    ///   contain file or directory path information. These operations are performed in a cross-platform manner.
    ///   
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/summary/*' />
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     <c>TxPath</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.Path" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </para>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    /// </remarks>
    [Serializable]
    public sealed class TxPath : IPath
    {
        internal TxPath(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="FileSystem"]/*' />
        public IFileSystem FileSystem => this.TxFileSystem;

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
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