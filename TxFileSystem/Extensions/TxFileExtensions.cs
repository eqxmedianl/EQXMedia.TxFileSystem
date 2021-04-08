namespace EQXMedia.TxFileSystem.Backups
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal static class TxFileExtensions
    {
        public static string GetBackupPath(this TxFile txFile, string path, Guid tempFileUuid)
        {
            var backupDirectory = txFile._txFileSystem.FileSystem.DirectoryInfo.FromDirectoryName(path).Parent.FullName
                + txFile._txFileSystem.FileSystem.Path.DirectorySeparatorChar;
            var backupFile = backupDirectory + "tempfile_" + tempFileUuid + "_"
                + txFile._txFileSystem.FileSystem.FileInfo.FromFileName(path).Name;

            return backupFile;
        }
    }
}
