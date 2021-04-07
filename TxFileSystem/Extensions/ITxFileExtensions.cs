namespace EQXMedia.TxFileSystem.Backups
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;

    internal static class ITxFileExtensions
    {
        public static string GetBackupPath(this ITxFile txFile, string path, Guid tempFileUuid)
        {
            var backupDirectory = txFile.TxFileSystem.FileSystem.DirectoryInfo.FromDirectoryName(path).Parent.FullName
                + txFile.TxFileSystem.FileSystem.Path.DirectorySeparatorChar;
            var backupFile = backupDirectory + "tempfile_" + tempFileUuid + "_"
                + txFile.TxFileSystem.FileSystem.FileInfo.FromFileName(path).Name;

            return backupFile;
        }
    }
}
