namespace EQXMedia.TxFileSystem.IO.Extensions
{
    using System;
    using System.IO;

    internal static class TxDirectoryExtensions
    {
        public static void Copy(this TxDirectory @this, string sourceDirPath, string destDirPath,
            bool preserveProperties = true)
        {
            @this.TxFileSystem.FileSystem.Directory.CreateDirectory(destDirPath);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var accessControl = @this.TxFileSystem.FileSystem.Directory.GetAccessControl(sourceDirPath);
                @this.TxFileSystem.FileSystem.Directory.SetAccessControl(destDirPath, accessControl);
            }

            if (preserveProperties)
            {
                PreserveTime(@this, sourceDirPath, destDirPath);
            }

            foreach (string dirPath in @this.TxFileSystem.FileSystem.Directory.GetDirectories(sourceDirPath, "*",
                SearchOption.AllDirectories))
            {
                @this.Copy(dirPath, dirPath
                    .Replace(@this.TxFileSystem.FileSystem.Path.GetFullPath(sourceDirPath), @this.TxFileSystem.FileSystem.Path.GetFullPath(destDirPath)),
                    preserveProperties);
            }

            foreach (string newPath in @this.TxFileSystem.FileSystem.Directory.GetFiles(sourceDirPath, "*",
                SearchOption.AllDirectories))
            {
                @this.FileSystem.File.Copy(newPath, newPath
                    .Replace(@this.TxFileSystem.FileSystem.Path.GetFullPath(sourceDirPath), @this.TxFileSystem.FileSystem.Path.GetFullPath(destDirPath)),
                    true);
            }
        }

        public static void CopyRecursive(this TxDirectory @this, string sourceDirPath, string destDirPath,
            bool preserveProperties = true)
        {
            @this.Copy(sourceDirPath, destDirPath, preserveProperties);
        }

        private static void PreserveTime(TxDirectory @this, string sourceDirName, string destDirName)
        {
            @this.TxFileSystem.FileSystem.Directory.SetCreationTimeUtc(destDirName,
                @this.TxFileSystem.FileSystem.Directory.GetCreationTimeUtc(sourceDirName));

            @this.TxFileSystem.FileSystem.Directory.SetLastAccessTimeUtc(destDirName,
                @this.TxFileSystem.FileSystem.Directory.GetLastAccessTimeUtc(sourceDirName));

            @this.TxFileSystem.FileSystem.Directory.SetLastWriteTimeUtc(destDirName,
                @this.TxFileSystem.FileSystem.Directory.GetLastWriteTimeUtc(sourceDirName));
        }
    }
}
