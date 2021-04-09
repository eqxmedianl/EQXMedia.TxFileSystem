namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public sealed class EnumerateDirectoriesOperation_Should
    {
        [Fact]
        public void EnumerateDirectoriesOperation_ReturnsThreeSubdirs()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/enumdir");
            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.Directory.CreateDirectory("/var/enumdir/subdir_" + i.ToString());
            }
            var directories = txFileSystem.Directory.EnumerateDirectories("/var/enumdir");

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 3);
        }

        [Fact]
        public void EnumerateDirectoriesOperation_FilterSubdirTwo_ReturnsOneSubdir()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/enumdir");
            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.Directory.CreateDirectory("/var/enumdir/subdir_" + i.ToString());
            }
            var directories = txFileSystem.Directory.EnumerateDirectories("/var/enumdir", "*_2");

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 1);
        }

        [Fact]
        public void EnumerateDirectoriesOperation_FilterSubdirUnderscore_ReturnsThreeSubdirs()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/enumdir");
            for (var i = 1; i <= 3; i++)
            {
                txFileSystem.Directory.CreateDirectory("/var/enumdir/subdir_" + i.ToString());
            }
            var directories = txFileSystem.Directory.EnumerateDirectories("/var/enumdir", "*_*");

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 3);
        }

        [Fact]
        public void EnumerateDirectoriesOperation_FilterSubdirUnderscore_AllDirectories_ReturnsThreeSubdirs()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/enumdir");
            for (var i = 1; i <= 3; i++)
            {
                var subdirPath = "/var/enumdir/subdir_" + i.ToString();
                txFileSystem.Directory.CreateDirectory(subdirPath);
                txFileSystem.Directory.CreateDirectory(subdirPath + "/subdir_childdir");
            }
            var directories = txFileSystem.Directory.EnumerateDirectories("/var/enumdir", "*_*",
                SearchOption.AllDirectories);

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 6);
        }

        [Fact]
        public void EnumerateDirectoriesOperation_FilterSubdirUnderscore_TopDirectories_ReturnsThreeSubdirs()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/enumdir");
            for (var i = 1; i <= 3; i++)
            {
                var subdirPath = "/var/enumdir/subdir_" + i.ToString();
                txFileSystem.Directory.CreateDirectory(subdirPath);
                txFileSystem.Directory.CreateDirectory(subdirPath + "/subdir_childdir");
            }
            var directories = txFileSystem.Directory.EnumerateDirectories("/var/enumdir", "*_*",
                SearchOption.TopDirectoryOnly);

            Assert.NotEmpty(directories);
            Assert.True(directories.Count() == 3);
        }

#if NETCOREAPP3_1_OR_GREATER
        [Fact, FsFact]
        public void EnumerateDirectoriesOperation_WithEnumerationOptions_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var dirName = "/tmp";
            var searchPattern = "*";
            var enumerationOptions = new EnumerationOptions()
            {
                MatchCasing = MatchCasing.CaseInsensitive
            };

            fileSystemMock.Setup(f => f.Directory.EnumerateDirectories(It.Is<string>((s) => s == dirName),
                It.Is<string>((p) => p == searchPattern), It.Is<EnumerationOptions>((o) => o == enumerationOptions)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.Directory.EnumerateDirectories(dirName, searchPattern, enumerationOptions);

            fileSystemMock.Verify(f => f.Directory.EnumerateDirectories(It.Is<string>((s) => s == dirName),
                It.Is<string>((p) => p == searchPattern), It.Is<EnumerationOptions>((o) => o == enumerationOptions)),
                Times.Once);
        }
#endif
    }
}
