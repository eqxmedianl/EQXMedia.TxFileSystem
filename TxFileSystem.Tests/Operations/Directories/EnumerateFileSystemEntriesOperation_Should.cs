namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using Xunit;

    public sealed class EnumerateFileSystemEntriesOperation_Should
    {
        [Fact]
        public void EnumerateFileSystemEntriesOperation_ReturnsSixEntries()
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

            var entries = txFileSystem.Directory.EnumerateFileSystemEntries("/var/filesysentriesdir");

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 6);
        }

        [Fact]
        public void EnumerateFileSystemEntriesOperation_FilterTxt_ReturnsThreeEntries()
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

            var entries = txFileSystem.Directory.EnumerateFileSystemEntries("/var/filesysentriesdir",
                "*.txt");

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 3);
        }

        [Fact]
        public void EnumerateFileSystemEntriesOperation_AllSubDirs_ReturnsTwentyFourEntries()
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

            var entries = txFileSystem.Directory.EnumerateFileSystemEntries("/var/filesysentriesdir",
                "*", System.IO.SearchOption.AllDirectories);

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 24);
        }

        [Fact]
        public void EnumerateFileSystemEntriesOperation_TopDirs_ReturnsSixEntries()
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

            var entries = txFileSystem.Directory.EnumerateFileSystemEntries("/var/filesysentriesdir",
                "*", System.IO.SearchOption.TopDirectoryOnly);

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 6);
        }

#if NETCOREAPP3_1_OR_GREATER
        [Fact]
        public void EnumerateFileSystemEntriesOperation_RecurseSubDirs_ReturnsTwentyFourEntries()
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

            var entries = txFileSystem.Directory.EnumerateFileSystemEntries("/var/filesysentriesdir",
                "*", new EnumerationOptions()
                {
                    RecurseSubdirectories = true
                });

            Assert.NotEmpty(entries);
            Assert.True(entries.Count() == 24);
        }
#endif
    }
}
