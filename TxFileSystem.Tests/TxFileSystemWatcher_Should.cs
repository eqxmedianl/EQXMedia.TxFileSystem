namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using Xunit;

    public sealed class TxFileSystemWatcher_Should
    {
        [Fact]
        public void TxFileSystemWatcher_FileSystem_SameFileSystemReturned()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.Equal(mockFileSystem, ((ITxFileSystemWatcher)txFileSystem.FileSystemWatcher).FileSystem);
        }

        [Fact, FsFact]
        public void TxFileSystemWatcher_CreateNew_ReturnsFileSystemWatcher()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var mockFileSystemWatcherFactory = new Mock<IFileSystemWatcherFactory>();
            fileSystemMock.SetupGet(f => f.FileSystemWatcher)
                .Returns(mockFileSystemWatcherFactory.Object);
            var mockFileSystemWatcher = new Mock<IFileSystemWatcher>();
            mockFileSystemWatcherFactory.Setup(f => f.CreateNew())
                .Returns(mockFileSystemWatcher.Object);
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileSystemWatcherReturned = txFileSystem.FileSystemWatcher.CreateNew();

            mockFileSystemWatcherFactory.Verify(f => f.CreateNew(), Times.Once);

            Assert.IsAssignableFrom<IFileSystemWatcher>(fileSystemWatcherReturned);
            Assert.Equal(mockFileSystemWatcher.Object, fileSystemWatcherReturned);
        }

        [Fact, FsFact]
        public void TxFileSystemWatcher_CreateNew_WithPath_ReturnsFileSystemWatcher()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var pathToWatch = "/tmp/dirtowatch";

            var mockFileSystemWatcherFactory = new Mock<IFileSystemWatcherFactory>();
            fileSystemMock.SetupGet(f => f.FileSystemWatcher)
                .Returns(mockFileSystemWatcherFactory.Object);
            var mockFileSystemWatcher = new Mock<IFileSystemWatcher>();
            mockFileSystemWatcherFactory.Setup(f => f.CreateNew(It.Is<string>((p) => p == pathToWatch)))
                .Returns(mockFileSystemWatcher.Object);
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileSystemWatcherReturned = txFileSystem.FileSystemWatcher.CreateNew(pathToWatch);

            mockFileSystemWatcherFactory.Verify(f => f.CreateNew(It.Is<string>((p) => p == pathToWatch)),
                Times.Once);

            Assert.IsAssignableFrom<IFileSystemWatcher>(fileSystemWatcherReturned);
            Assert.Equal(mockFileSystemWatcher.Object, fileSystemWatcherReturned);
        }

        [Fact, FsFact]
        public void TxFileSystemWatcher_CreateNew_WithPathAndFilter_ReturnsFileSystemWatcher()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var pathToWatch = "/tmp/dirtowatch";
            var watchFilter = "*.out";

            var mockFileSystemWatcherFactory = new Mock<IFileSystemWatcherFactory>();
            fileSystemMock.SetupGet(f => f.FileSystemWatcher)
                .Returns(mockFileSystemWatcherFactory.Object);
            var mockFileSystemWatcher = new Mock<IFileSystemWatcher>();
            mockFileSystemWatcherFactory.Setup(f => f.CreateNew(It.Is<string>((p) => p == pathToWatch),
                It.Is<string>((p) => p == watchFilter)))
                .Returns(mockFileSystemWatcher.Object);
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var fileSystemWatcherReturned = txFileSystem.FileSystemWatcher.CreateNew(pathToWatch, watchFilter);

            mockFileSystemWatcherFactory.Verify(f => f.CreateNew(It.Is<string>((p) => p == pathToWatch),
                It.Is<string>((p) => p == watchFilter)), Times.Once);

            Assert.IsAssignableFrom<IFileSystemWatcher>(fileSystemWatcherReturned);
            Assert.Equal(mockFileSystemWatcher.Object, fileSystemWatcherReturned);
        }
    }
}
