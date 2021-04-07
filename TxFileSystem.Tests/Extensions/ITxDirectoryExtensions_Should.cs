namespace EQXMedia.TxFileSystem.Tests.Extensions
{
    using global::EQXMedia.TxFileSystem.IO.Extensions;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using Xunit;

    public sealed class TxDirectoryInterfaceExtensions_Should
    {
        [Fact]
        public void TxDirectoryInterfaceExtensions_CopyRecursive()
        {
            var sourceDirPath = "/tmp/sourcedir";
            var destDirPath = "/tmp/destdir";

            var fileOneFileName = sourceDirPath + "/file1.txt";
            var fileTwoFileName = sourceDirPath + "/file2.doc";
            var fileThreeFileName = sourceDirPath + "/file3.cs";

            var subDirOnePath = sourceDirPath + "/subdir1";
            var subDirTwoPath = sourceDirPath + "/subdir2";

            var subDirTwoFileOneFileName = subDirOnePath + "/file1.md";
            var subDirTwoFileTwoFileName = subDirTwoPath + "/file2.rtf";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");

            txFileSystem.Directory.CreateDirectory(sourceDirPath);
            txFileSystem.Directory.SetCreationTime(sourceDirPath, DateTime.Now.AddDays(-7));
            txFileSystem.File.CreateText(fileOneFileName);
            txFileSystem.File.Create(fileTwoFileName);
            txFileSystem.File.CreateText(fileThreeFileName);

            txFileSystem.Directory.CreateDirectory(subDirOnePath);
            txFileSystem.Directory.SetLastAccessTime(subDirOnePath, DateTime.Now.AddDays(-7));

            txFileSystem.Directory.CreateDirectory(subDirTwoPath);
            txFileSystem.Directory.SetLastWriteTime(subDirTwoPath, DateTime.Now.AddDays(-7));
            txFileSystem.File.CreateText(subDirTwoFileOneFileName);
            txFileSystem.File.Create(subDirTwoFileTwoFileName);

            var txDirectory = new TxDirectory(txFileSystem);

            txDirectory.CopyRecursive(sourceDirPath, destDirPath, preserveProperties: true);

            Assert.True(txFileSystem.Directory.Exists(destDirPath));
            Assert.True(txFileSystem.File.Exists(DestDirPath(fileOneFileName, sourceDirPath, destDirPath)));
            Assert.True(txFileSystem.File.Exists(DestDirPath(fileTwoFileName, sourceDirPath, destDirPath)));
            Assert.True(txFileSystem.File.Exists(DestDirPath(fileThreeFileName, sourceDirPath, destDirPath)));

            Assert.True(txFileSystem.Directory.Exists(DestDirPath(subDirOnePath, sourceDirPath, destDirPath)));

            Assert.True(txFileSystem.Directory.Exists(DestDirPath(subDirTwoPath, sourceDirPath, destDirPath)));
            Assert.True(txFileSystem.File.Exists(DestDirPath(subDirTwoFileOneFileName, sourceDirPath, destDirPath)));
            Assert.True(txFileSystem.File.Exists(DestDirPath(subDirTwoFileTwoFileName, sourceDirPath, destDirPath)));
        }

        private string DestDirPath(string subDirOnePath, string sourceDirPath, string destDirPath)
        {
            return subDirOnePath.Replace(sourceDirPath, destDirPath);
        }
    }
}
