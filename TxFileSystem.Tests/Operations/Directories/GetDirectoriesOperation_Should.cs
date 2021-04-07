namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using Xunit;

    public sealed class GetDirectoriesOperation_Should
    {
        [Fact]
        public void GetDirectoriesOperation_ReturnsThreeDirNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirsdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirsdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i + "/subsubdir_" + j);
                    txFileSystem.File.Create("/var/dirsdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var directories = txFileSystem.Directory.GetDirectories("/var/dirsdir");

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 3);
        }

        [Fact]
        public void GetDirectoriesOperation_FilterTwo_ReturnsOneDirName()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirsdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirsdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i + "/subsubdir_" + j);
                    txFileSystem.File.Create("/var/dirsdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var directories = txFileSystem.Directory.GetDirectories("/var/dirsdir",
                "*_2");

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 1);
        }

        [Fact]
        public void GetDirectoriesOperation_AllSubDirs_ReturnsTwentyOneDirNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirsdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirsdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i + "/subsubdir_" + j);
                    txFileSystem.File.Create("/var/dirsdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var directories = txFileSystem.Directory.GetDirectories("/var/dirsdir",
                "*", System.IO.SearchOption.AllDirectories);

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 21);
        }

        [Fact]
        public void GetDirectoriesOperation_TopDirs_ReturnsThreeDirNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirsdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirsdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i + "/subsubdir_" + j);
                    txFileSystem.File.Create("/var/dirsdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var directories = txFileSystem.Directory.GetDirectories("/var/dirsdir",
                "*", System.IO.SearchOption.TopDirectoryOnly);

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 3);
        }

        [Fact]
        public void GetDirectoriesOperation_RecurseSubDirs_ReturnsTwentyOneDirNames()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirsdir");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirsdir/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.Directory.CreateDirectory("/var/dirsdir/subdir_" + i + "/subsubdir_" + j);
                    txFileSystem.File.Create("/var/dirsdir/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var directories = txFileSystem.Directory.GetDirectories("/var/dirsdir",
                "*", new EnumerationOptions()
                {
                    RecurseSubdirectories = true
                });

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 21);
        }
    }
}
