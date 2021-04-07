namespace EQXMedia.TxFileSystem
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions;

    internal sealed class TxPath : ITxPath
    {
        public TxPath(ITxFileSystem txFileSystem)
        {
            ((ITxPath)this).TxFileSystem = txFileSystem;
        }

        public IFileSystem FileSystem => ((ITxPath)this).TxFileSystem.FileSystem;

        ITxFileSystem ITxPath.TxFileSystem { get; set; }

        public char AltDirectorySeparatorChar => this.FileSystem.Path.AltDirectorySeparatorChar;

        public char DirectorySeparatorChar => this.FileSystem.Path.DirectorySeparatorChar;

        public char PathSeparator => this.FileSystem.Path.PathSeparator;

        public char VolumeSeparatorChar => this.FileSystem.Path.VolumeSeparatorChar;

        public char[] InvalidPathChars => this.FileSystem.Path.InvalidPathChars;

        public string ChangeExtension(string path, string extension)
        {
            return this.FileSystem.Path.ChangeExtension(path, extension);
        }

        public string Combine(params string[] paths)
        {
            return this.FileSystem.Path.Combine(paths);
        }

        public string Combine(string path1, string path2)
        {
            return this.FileSystem.Path.Combine(path1, path2);
        }

        public string Combine(string path1, string path2, string path3)
        {
            return this.FileSystem.Path.Combine(path1, path2, path3);
        }

        public string Combine(string path1, string path2, string path3, string path4)
        {
            return this.FileSystem.Path.Combine(path1, path2, path3, path4);
        }

        public string GetDirectoryName(string path)
        {
            return this.FileSystem.Path.GetDirectoryName(path);
        }

        public string GetExtension(string path)
        {
            return this.FileSystem.Path.GetExtension(path);
        }

        public string GetFileName(string path)
        {
            return this.FileSystem.Path.GetFileName(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return this.FileSystem.Path.GetFileNameWithoutExtension(path);
        }

        public string GetFullPath(string path)
        {
            return this.FileSystem.Path.GetFullPath(path);
        }

        public string GetFullPath(string path, string basePath)
        {
            return this.FileSystem.Path.GetFullPath(path, basePath);
        }

        public char[] GetInvalidFileNameChars()
        {
            return this.FileSystem.Path.GetInvalidFileNameChars();
        }

        public char[] GetInvalidPathChars()
        {
            return this.FileSystem.Path.GetInvalidPathChars();
        }

        public string GetPathRoot(string path)
        {
            return this.FileSystem.Path.GetPathRoot(path);
        }

        public string GetRandomFileName()
        {
            return this.FileSystem.Path.GetRandomFileName();
        }

        public string GetRelativePath(string relativeTo, string path)
        {
            return this.FileSystem.Path.GetRelativePath(relativeTo, path);
        }

        public string GetTempFileName()
        {
            return this.FileSystem.Path.GetTempFileName();
        }

        public string GetTempPath()
        {
            return this.FileSystem.Path.GetTempPath();
        }

        public bool HasExtension(string path)
        {
            return this.FileSystem.Path.HasExtension(path);
        }

        public bool IsPathFullyQualified(string path)
        {
            return this.FileSystem.Path.IsPathFullyQualified(path);
        }

        public bool IsPathRooted(string path)
        {
            return this.FileSystem.Path.IsPathRooted(path);
        }

        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2)
        {
            return this.FileSystem.Path.Join(path1, path2);
        }

        public string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3)
        {
            return this.FileSystem.Path.Join(path1, path2, path3);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, 
            Span<char> destination, out int charsWritten)
        {
            return this.FileSystem.Path.TryJoin(path1, path2, path3, destination, out charsWritten);
        }

        public bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination,
            out int charsWritten)
        {
            return this.FileSystem.Path.TryJoin(path1, path2, destination, out charsWritten);
        }
    }
}