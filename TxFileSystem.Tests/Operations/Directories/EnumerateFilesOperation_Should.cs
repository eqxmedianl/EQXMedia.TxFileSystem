namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using Xunit;

    public sealed class EnumerateFilesOperation_Should
    {
        [Fact]
        public void EnumerateFilesOperation_EnumerateFiles_ReturnsThreeFiles()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirtoenum/");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirtoenum/file_" + i + ".txt");
            }

            var files = txFileSystem.Directory.EnumerateFiles("/var/dirtoenum/");

            Assert.NotEmpty(files);
            Assert.True(files.Count() == 3);
        }

        [Fact]
        public void EnumerateFilesOperation_EnumerateFiles_Filtered_ReturnsOneFile()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirtoenum/");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirtoenum/file_" + i + ".txt");
            }

            var files = txFileSystem.Directory.EnumerateFiles("/var/dirtoenum/", "*_2.txt");

            Assert.NotEmpty(files);
            Assert.True(files.Count() == 1);
        }

        [Fact]
        public void EnumerateFilesOperation_EnumerateFiles_FilteredPlusTopDir_ReturnsThreeFiles()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirtoenum/");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirtoenum/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirtoenum/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/dirtoenum/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var files = txFileSystem.Directory.EnumerateFiles("/var/dirtoenum/", "*",
                SearchOption.TopDirectoryOnly);

            Assert.NotEmpty(files);
            Assert.True(files.Count() == 3);
        }

        [Fact]
        public void EnumerateFilesOperation_EnumerateFiles_FilteredPlusAllDirs_ReturnsTwentyOneFiles()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirtoenum/");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirtoenum/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirtoenum/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/dirtoenum/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var files = txFileSystem.Directory.EnumerateFiles("/var/dirtoenum/", "*",
                SearchOption.AllDirectories);

            Assert.NotEmpty(files);
            Assert.True(files.Count() == 21);
        }

        [Fact]
        public void EnumerateFilesOperation_EnumerateFiles_FilterCaseSensitive_ReturnsNoFiles()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirtoenum/");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirtoenum/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirtoenum/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/dirtoenum/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var files = txFileSystem.Directory.EnumerateFiles("/var/dirtoenum/", "subFile_*",
                new EnumerationOptions()
                {
                    MatchCasing = MatchCasing.CaseSensitive
                });

            Assert.Empty(files);
        }

        [Fact]
        public void EnumerateFilesOperation_EnumerateFiles_FilterPlusRecurseSubdirectories_ReturnsThreeFiles()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/dirtoenum/");

            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.File.Create("/var/dirtoenum/file_" + i + ".txt");
                txFileSystem.Directory.CreateDirectory("/var/dirtoenum/subdir_" + i);

                for (var j = 1; j <= 6; j++)
                {
                    txFileSystem.File.Create("/var/dirtoenum/subdir_" + i + "/subfile_" + j + ".txt");
                }
            }

            var files = txFileSystem.Directory.EnumerateFiles("/var/dirtoenum/", "*",
                new EnumerationOptions()
                {
                    RecurseSubdirectories = false
                });

            Assert.NotEmpty(files);
            Assert.True(files.Count() == 3);
        }
    }
}
