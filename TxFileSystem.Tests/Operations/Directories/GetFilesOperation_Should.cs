namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using Xunit;

    public sealed class GetFilesOperation_Should
    {
        [Fact]
        public void GetFilesOperation_ReturnsThreeFileNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var fileNames = txFileSystem.Directory.GetFiles("/var/filesdir");

            Assert.NotEmpty(fileNames);
            Assert.True(fileNames.Count() == 3);
        }

        [Fact]
        public void GetFilesOperation_FilterTxt_ReturnsThreeFileNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var fileNames = txFileSystem.Directory.GetFiles("/var/filesdir",
                "*.txt");

            Assert.NotEmpty(fileNames);
            Assert.True(fileNames.Count() == 3);
        }

        [Fact]
        public void GetFilesOperation_AllSubDirs_ReturnsTwentyOneFileNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var fileNames = txFileSystem.Directory.GetFiles("/var/filesdir",
                "*", System.IO.SearchOption.AllDirectories);

            Assert.NotEmpty(fileNames);
            Assert.True(fileNames.Count() == 21);
        }

        [Fact]
        public void GetFilesOperation_TopDirs_ReturnsThreeFileNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var fileNames = txFileSystem.Directory.GetFiles("/var/filesdir",
                "*", System.IO.SearchOption.TopDirectoryOnly);

            Assert.NotEmpty(fileNames);
            Assert.True(fileNames.Count() == 3);
        }

        [Fact]
        public void GetFilesOperation_RecurseSubDirs_ReturnsTwentyOneFileNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/filesdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/filesdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/filesdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/filesdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var fileNames = txFileSystem.Directory.GetFiles("/var/filesdir",
                "*", new EnumerationOptions()
                {
                    RecurseSubdirectories = true
                });

            Assert.NotEmpty(fileNames);
            Assert.True(fileNames.Count() == 21);
        }
    }
}
