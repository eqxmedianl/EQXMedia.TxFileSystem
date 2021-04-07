namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using System.Transactions;
    using Xunit;

    public sealed class DirectoryBackupOperation_Should
    {
        [Fact]
        public void DirectoryBackupOperation_Backup_BackupPathExists_ReturnsTrue()
        {
            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp/directorytobackupped");

            transactionScope.Complete();

            var unitTestOperation = new UnitTestDirectoryOperation((ITxDirectory)txFileSystem.Directory,
                "/tmp/directorytobackupped");
            unitTestOperation.Backup();

            Assert.True(txFileSystem.Directory.Exists(unitTestOperation.BackupPath));
        }

        [Fact]
        public void DirectoryBackupOperation_Backup_BackupPath_SubDirectoriesExist()
        {
            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp/directorytobackupped");
            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.Directory.CreateDirectory("/tmp/directorytobackupped/subdir_" + i.ToString());
            }

            transactionScope.Complete();

            var unitTestOperation = new UnitTestDirectoryOperation((ITxDirectory)txFileSystem.Directory,
                "/tmp/directorytobackupped");
            unitTestOperation.Backup();

            var directories = txFileSystem.Directory.EnumerateDirectories(unitTestOperation.BackupPath);

            Assert.NotEmpty(directories);
            Assert.Equal(3, directories.Count());
        }

        [Fact]
        public void DirectoryBackupOperation_Backup_BackupPath_FilesExist()
        {
            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp/directorytobackupped");
            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/tmp/directorytobackupped/rootfile_" + i.ToString());
                txFileSystem.Directory.CreateDirectory("/tmp/directorytobackupped/subdir_" + i.ToString());
                for (var j = 1; j <= 3; j++)
                {
                    txFileSystem.File.Create("/tmp/directorytobackupped/subdir_" + i.ToString() + 
                        "/subfile_" + i.ToString());
                }
            }

            var unitTestOperation = new UnitTestDirectoryOperation((ITxDirectory)txFileSystem.Directory,
                "/tmp/directorytobackupped");
            unitTestOperation.Backup();

            var directories = txFileSystem.Directory.EnumerateDirectories(unitTestOperation.BackupPath);

            Assert.NotEmpty(directories);
            Assert.Equal(3, directories.Count());

            var files = txFileSystem.Directory.EnumerateFiles(unitTestOperation.BackupPath, "*",
                SearchOption.AllDirectories);

            Assert.NotEmpty(files);
            Assert.Equal(6, files.Count());
        }
    }
}
