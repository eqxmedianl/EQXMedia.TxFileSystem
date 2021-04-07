namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using Xunit;

    public sealed class GetFileSystemEntriesOperation_Should
    {
        [Fact]
        public void GetFileSystemEntriesOperation_ReturnsSixEntries()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesysentriesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesysentriesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesysentriesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesysentriesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var entries = txFileSystem.Directory.GetFileSystemEntries("/var/filesysentriesdir");

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 6);
        }

        [Fact]
        public void GetFileSystemEntriesOperation_FilterTxt_ReturnsThreeEntries()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesysentriesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesysentriesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesysentriesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesysentriesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var entries = txFileSystem.Directory.GetFileSystemEntries("/var/filesysentriesdir",
                "*.txt");

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 3);
        }
    }
}
