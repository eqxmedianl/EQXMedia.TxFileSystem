namespace EQXMedia.TxFileSystem
{
    using System;
    using System.IO.Abstractions;

    /// <summary>
    ///   Transactional path exposes methods to perform operations on <see cref="System.String"/> instances that 
    ///   contain file or directory path information. These operations are performed in a cross-platform manner.
    /// </summary>
    /// <remarks>
    ///   <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@modifying="false"]/remarks/*' />
    ///   <note type="note">
    ///     <c>TxPath</c> is used underlying at <see cref="EQXMedia.TxFileSystem.TxFileSystem.Path" />. 
    ///     It can't be used without a <see cref="EQXMedia.TxFileSystem.TxFileSystem" /> instance.
    ///   </note>
    /// </remarks>
    [Serializable]
    public sealed class TxPath : IPath
    {
        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@context="constructor" and @param="TxFileSystem"]/*' />
        internal TxPath(TxFileSystem txFileSystem)
        {
            this.TxFileSystem = txFileSystem;
        }

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="FileSystem"]/*' />
        public IFileSystem FileSystem => this.TxFileSystem;

        /// <include file="../Documentation/XmlDoc/TxFileSystem.XmlDoc.Extensions.xml" path='TxFileSystem.BaseDocs/Extensions/Classes/Class[@property="TxFileSystem"]/*' />
        internal TxFileSystem TxFileSystem { get; set; }

        /// <inheritdoc cref="System.IO.Path.AltDirectorySeparatorChar" />
        public char AltDirectorySeparatorChar => this.TxFileSystem.FileSystem.Path.AltDirectorySeparatorChar;

        /// <inheritdoc cref="System.IO.Path.DirectorySeparatorChar" />
        public char DirectorySeparatorChar => this.TxFileSystem.FileSystem.Path.DirectorySeparatorChar;

        /// <inheritdoc cref="System.IO.Path.PathSeparator" />
        public char PathSeparator => this.TxFileSystem.FileSystem.Path.PathSeparator;

        /// <inheritdoc cref="System.IO.Path.VolumeSeparatorChar" />
        public char VolumeSeparatorChar => this.TxFileSystem.FileSystem.Path.VolumeSeparatorChar;

        /// <inheritdoc cref="System.IO.Path.InvalidPathChars" />
        public char[] InvalidPathChars => this.TxFileSystem.FileSystem.Path.InvalidPathChars;

        /// <inheritdoc cref="System.IO.Path.ChangeExtension(string, string)" />
        public string ChangeExtension(string path, string extension)
        {
            return this.TxFileSystem.FileSystem.Path.ChangeExtension(path, extension);
        }

        /// <inheritdoc cref="System.IO.Path.Combine(string[])" />
        public string Combine(params string[] paths)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(paths);
        }

        /// <inheritdoc cref="System.IO.Path.Combine(string, string)" />
        public string Combine(string path1, string path2)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(path1, path2);
        }

        /// <inheritdoc cref="System.IO.Path.Combine(string, string, string)" />
        public string Combine(string path1, string path2, string path3)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(path1, path2, path3);
        }

        /// <inheritdoc cref="System.IO.Path.Combine(string, string, string, string)" />
        public string Combine(string path1, string path2, string path3, string path4)
        {
            return this.TxFileSystem.FileSystem.Path.Combine(path1, path2, path3, path4);
        }

        /// <inheritdoc cref="System.IO.Path.GetDirectoryName(string)" />
        public string GetDirectoryName(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetDirectoryName(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetExtension(string)" />
        public string GetExtension(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetExtension(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetFileName(string)" />
        public string GetFileName(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFileName(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(string)" />
        public string GetFileNameWithoutExtension(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFileNameWithoutExtension(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetFullPath(string)" />
        public string GetFullPath(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFullPath(path);
        }

#if PATH_NG
        /// <inheritdoc cref="System.IO.Path.GetFullPath(string, string)" />
        public string GetFullPath(string path, string basePath)
        {
            return this.TxFileSystem.FileSystem.Path.GetFullPath(path, basePath);
        }
#endif

        /// <inheritdoc cref="System.IO.Path.GetInvalidFileNameChars()" />
        public char[] GetInvalidFileNameChars()
        {
            return this.TxFileSystem.FileSystem.Path.GetInvalidFileNameChars();
        }

        /// <inheritdoc cref="System.IO.Path.GetInvalidPathChars()" />
        public char[] GetInvalidPathChars()
        {
            return this.TxFileSystem.FileSystem.Path.GetInvalidPathChars();
        }

        /// <inheritdoc cref="System.IO.Path.GetPathRoot(string)" />
        public string GetPathRoot(string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetPathRoot(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetRandomFileName()" />
        public string GetRandomFileName()
        {
            return this.TxFileSystem.FileSystem.Path.GetRandomFileName();
        }

#if PATH_NG
        /// <inheritdoc cref="System.IO.Path.GetRelativePath(string, string)" />
        public string GetRelativePath(string relativeTo, string path)
        {
            return this.TxFileSystem.FileSystem.Path.GetRelativePath(relativeTo, path);
        }
#endif

        /// <inheritdoc cref="System.IO.Path.GetTempFileName()" />
        public string GetTempFileName()
        {
            return this.TxFileSystem.FileSystem.Path.GetTempFileName();
        }

        /// <inheritdoc cref="System.IO.Path.GetTempPath()" />
        public string GetTempPath()
        {
            return this.TxFileSystem.FileSystem.Path.GetTempPath();
        }

        /// <inheritdoc cref="System.IO.Path.HasExtension(string)" />
        public bool HasExtension(string path)
        {
            return this.TxFileSystem.FileSystem.Path.HasExtension(path);
        }

#if PATH_NG
        /// <inheritdoc cref="System.IO.Path.IsPathFullyQualified(string)" />
        public bool IsPathFullyQualified(string path)
        {
            return this.TxFileSystem.FileSystem.Path.IsPathFullyQualified(path);
        }
#endif

        /// <inheritdoc cref="System.IO.Path.IsPathRooted(string)" />
        public bool IsPathRooted(string path)
        {
            return this.TxFileSystem.FileSystem.Path.IsPathRooted(path);
        }

#if PATH_NG
        /// <inheritdoc cref="System.IO.Path.Join(ReadOnlySpan{char}, ReadOnlySpan{char})" />
        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2);
        }

        /// <inheritdoc cref="System.IO.Path.Join(ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char})" />
        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2, path3);
        }

        /// <inheritdoc cref="System.IO.Path.TryJoin(ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char}, Span{char}, out int)" />
        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3,
            Span<char> destination, out int charsWritten)
        {
            return this.TxFileSystem.FileSystem.Path.TryJoin(path1, path2, path3, destination, out charsWritten);
        }

        /// <inheritdoc cref="System.IO.Path.TryJoin(ReadOnlySpan{char}, ReadOnlySpan{char}, Span{char}, out int)" />
        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination,
            out int charsWritten)
        {
            return this.TxFileSystem.FileSystem.Path.TryJoin(path1, path2, destination, out charsWritten);
        }

#endif

#if !NETFRAMEWORK
#if !NETSTANDARD2_0
        /// <inheritdoc cref="System.IO.Path.HasExtension(ReadOnlySpan{char})" />
        public bool HasExtension(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.HasExtension(path);
        }

        /// <inheritdoc cref="System.IO.Path.IsPathFullyQualified(ReadOnlySpan{char})" />
        public bool IsPathFullyQualified(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.IsPathFullyQualified(path);
        }

        /// <inheritdoc cref="System.IO.Path.IsPathRooted(ReadOnlySpan{char})" />
        public bool IsPathRooted(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.IsPathRooted(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetDirectoryName(ReadOnlySpan{char})" />
        public ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.GetDirectoryName(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetExtension(ReadOnlySpan{char})" />
        public ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.GetExtension(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetFileName(ReadOnlySpan{char})" />
        public ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFileName(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(ReadOnlySpan{char})" />
        public ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.GetFileNameWithoutExtension(path);
        }

        /// <inheritdoc cref="System.IO.Path.GetPathRoot(ReadOnlySpan{char})" />
        public ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.GetPathRoot(path);
        }

#if !NETSTANDARD2_1
        /// <inheritdoc cref="System.IO.Path.Join(string, string)" />
#else
        /// <summary>
        ///   Concatenates two paths into a single path.
        /// </summary>
        /// <returns>The concatenated path.</returns>
#endif
        public string Join(string path1, string path2)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2);
        }

#if !NETSTANDARD2_1
        /// <inheritdoc cref="System.IO.Path.Join(string, string, string)" />
#else
        /// <summary>
        ///   Concatenates three paths into a single path.
        /// </summary>
        /// <returns>The concatenated path.</returns>
#endif
        public string Join(string path1, string path2, string path3)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2, path3);
        }

#if !NETSTANDARD2_1
        /// <inheritdoc cref="System.IO.Path.Join(string[])" />
        public string Join(params string[] paths)
        {
            return this.TxFileSystem.FileSystem.Path.Join(paths);
        }

        /// <inheritdoc cref="System.IO.Path.EndsInDirectorySeparator(ReadOnlySpan{char})" />
        public bool EndsInDirectorySeparator(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.EndsInDirectorySeparator(path);
        }

        /// <inheritdoc cref="System.IO.Path.EndsInDirectorySeparator(string)" />
        public bool EndsInDirectorySeparator(string path)
        {
            return this.TxFileSystem.FileSystem.Path.EndsInDirectorySeparator(path);
        }

        /// <inheritdoc cref="System.IO.Path.TrimEndingDirectorySeparator(ReadOnlySpan{char})" />
        public ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path)
        {
            return this.TxFileSystem.FileSystem.Path.TrimEndingDirectorySeparator(path);
        }

        /// <inheritdoc cref="System.IO.Path.TrimEndingDirectorySeparator(string)" />
        public string TrimEndingDirectorySeparator(string path)
        {
            return this.TxFileSystem.FileSystem.Path.TrimEndingDirectorySeparator(path);
        }

        /// <inheritdoc cref="System.IO.Path.Join(ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char})" />
        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, ReadOnlySpan<char> path4)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2, path3, path4);
        }

        /// <inheritdoc cref="System.IO.Path.Join(string, string, string, string)" />
        public string Join(string path1, string path2, string path3, string path4)
        {
            return this.TxFileSystem.FileSystem.Path.Join(path1, path2, path3, path4);
        }
#endif
#endif
#endif
    }
}