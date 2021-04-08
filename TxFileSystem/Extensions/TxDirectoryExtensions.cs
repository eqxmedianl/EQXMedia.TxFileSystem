namespace EQXMedia.TxFileSystem.IO.Extensions
{
    using System.IO;

    internal static class TxDirectoryExtensions
    {
        public static void Copy(this TxDirectory @this, string sourceDirPath, string destDirPath,
            bool preserveProperties = true)
        {
            @this.FileSystem.Directory.CreateDirectory(destDirPath);

            var accessControl = @this.FileSystem.Directory.GetAccessControl(sourceDirPath);
            @this.FileSystem.Directory.SetAccessControl(destDirPath, accessControl);

            if (preserveProperties)
            {
                PreserveTime(@this, sourceDirPath, destDirPath);
            }

            foreach (string dirPath in @this.FileSystem.Directory.GetDirectories(sourceDirPath, "*",
                SearchOption.AllDirectories))
            {
                @this.Copy(dirPath, dirPath
                    .Replace(@this.FileSystem.Path.GetFullPath(sourceDirPath), @this.FileSystem.Path.GetFullPath(destDirPath)),
                    preserveProperties);
            }

            foreach (string newPath in @this.FileSystem.Directory.GetFiles(sourceDirPath, "*",
                SearchOption.AllDirectories))
            {
                @this.FileSystem.File.Copy(newPath, newPath
                    .Replace(@this.FileSystem.Path.GetFullPath(sourceDirPath), @this.FileSystem.Path.GetFullPath(destDirPath)),
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
            @this.FileSystem.Directory.SetCreationTimeUtc(destDirName,
                @this.FileSystem.Directory.GetCreationTimeUtc(sourceDirName));

            @this.FileSystem.Directory.SetLastAccessTimeUtc(destDirName,
                @this.FileSystem.Directory.GetLastAccessTimeUtc(sourceDirName));

            @this.FileSystem.Directory.SetLastWriteTimeUtc(destDirName,
                @this.FileSystem.Directory.GetLastWriteTimeUtc(sourceDirName));
        }
    }
}
