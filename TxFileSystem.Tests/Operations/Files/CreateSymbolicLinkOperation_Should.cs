#if SYMBOLIC_LINKS
namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using Moq;
    using System;
#if SUPPRESS_SIMPLE_USING
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
#endif
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class CreateSymbolicLinkOperation_Should
    {
        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void CreateSymbolicLinkOperation_Fails_ResultsInExists_ReturnsFalse()
        {
            var parentDirectoryInfoMock = new Mock<IDirectoryInfo>();
            parentDirectoryInfoMock.SetupGet(d => d.FullName)
                .Returns("/var");

            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.SetupGet(d => d.Parent)
                .Returns(parentDirectoryInfoMock.Object);

            var directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            directoryInfoFactoryMock.Setup(d => d.FromDirectoryName(It.IsAny<string>()))
                .Returns(directoryInfoMock.Object);

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.SetupGet(f => f.Name)
                .Returns("failingsymlink");

            var fileInfoFactoryMock = new Mock<IFileInfoFactory>();
            fileInfoFactoryMock.Setup(p => p.FromFileName(It.IsAny<string>()))
                .Returns(fileInfoMock.Object);

            var pathMock = new Mock<IPath>();
            pathMock.SetupGet(p => p.DirectorySeparatorChar)
                .Returns(Path.DirectorySeparatorChar);

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.SetupGet(f => f.DirectoryInfo)
                .Returns(directoryInfoFactoryMock.Object);
            fileSystemMock.SetupGet(f => f.FileInfo)
                .Returns(fileInfoFactoryMock.Object);
            fileSystemMock.SetupGet(f => f.Path)
                .Returns(pathMock.Object);
            fileSystemMock.Setup(f => f.File.CreateSymbolicLink(
                    It.Is<string>((s) => s == "/var/failingsymlink"),
                    It.Is<string>((t) => t == "/var/failingpathtotarget"))
                )
                .Throws(new Exception("Failed to create failing symlink"));

            TxFileSystem txFileSystem = null;

            Assert.Throws<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(fileSystemMock.Object);
                    txFileSystem.File.Create("/var/failingpathtotarget");
                    txFileSystem.File.CreateSymbolicLink("/var/failingsymlink",
                        "/var/failingpathtotarget");
                    transactionScope.Complete();
                }
            });

            Assert.True(txFileSystem.Journal.IsRolledBack);

            fileSystemMock.Verify(f => f.File.Create(
                    It.Is<string>((s) => s == "/var/failingpathtotarget")),
                    Times.Once);
            fileSystemMock.Verify(f => f.File.CreateSymbolicLink(
                    It.Is<string>((s) => s == "/var/failingsymlink"),
                    It.Is<string>((t) => t == "/var/failingpathtotarget")),
                    Times.Once);
        }

        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void CreateSymbolicLinkOperation_ResultsInExists_ReturnsTrue()
        {
            TxFileSystem txFileSystem = null;

            void CreateSymbolicLink()
            {
                #region CodeExample_CreateSymbolicLink

                var mockFileSystem = new MockFileSystem();
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.CreateDirectory("/var");
                    txFileSystem.File.Create("/var/nonfailingpathtotarget");
                    txFileSystem.File.CreateSymbolicLink("/var/nonfailingsymlink",
                        "/var/nonfailingpathtotarget");
                    transactionScope.Complete();
                }

                #endregion
            }

            CreateSymbolicLink();

            var txJournal = txFileSystem.Journal;

            Assert.Equal(JournalState.Committed, txJournal.State);
            Assert.False(txJournal.IsRolledBack);
            Assert.True(txFileSystem.Directory.Exists("/var"));
            Assert.True(txFileSystem.File.Exists("/var/nonfailingpathtotarget"));
            Assert.True(txFileSystem.File.Exists("/var/nonfailingsymlink"));
        }

        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void CreateSymbolicLinkOperation_OnExistingSymlink_ExceptionThrown_ResultsInExists_ReturnsTrue()
        {
            var parentDirectoryInfoMock = new Mock<IDirectoryInfo>();
            parentDirectoryInfoMock.SetupGet(d => d.FullName)
                .Returns("/var");

            var directoryInfoMock = new Mock<IDirectoryInfo>();
            directoryInfoMock.SetupGet(d => d.Parent)
                .Returns(parentDirectoryInfoMock.Object);

            var directoryInfoFactoryMock = new Mock<IDirectoryInfoFactory>();
            directoryInfoFactoryMock.Setup(d => d.FromDirectoryName(It.IsAny<string>()))
                .Returns(directoryInfoMock.Object);

            var fileMock = new Mock<IFile>();
            fileMock.Setup(f => f.CreateSymbolicLink(It.IsAny<string>(),
                It.IsAny<string>()));

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.SetupGet(f => f.Name)
                .Returns("failingsymlink");

            var fileInfoFactoryMock = new Mock<IFileInfoFactory>();
            fileInfoFactoryMock.Setup(p => p.FromFileName(It.IsAny<string>()))
                .Returns(fileInfoMock.Object);

            var pathMock = new Mock<IPath>();
            pathMock.SetupGet(p => p.DirectorySeparatorChar)
                .Returns(Path.DirectorySeparatorChar);

            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.SetupGet(f => f.DirectoryInfo)
                .Returns(directoryInfoFactoryMock.Object);
            fileSystemMock.SetupGet(f => f.File)
                .Returns(fileMock.Object);
            fileSystemMock.SetupGet(f => f.FileInfo)
                .Returns(fileInfoFactoryMock.Object);
            fileSystemMock.SetupGet(f => f.Path)
                .Returns(pathMock.Object);
            fileSystemMock.Setup(f => f.File.CreateSymbolicLink(
                    It.Is<string>((s) => s == "/var/failingsymlink"),
                    It.Is<string>((t) => t == "/var/failingpathtotarget"))
                )
                .Throws(new Exception("Failed to create failing symlink"));

            TxFileSystem txFileSystem = null;

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(fileSystemMock.Object);
                    txFileSystem.File.CreateSymbolicLink("/var/alreadyexistingsymlink",
                        "/var/pathtotarget");

                    throw new Exception("Error occurs after calling CreateSymbolicLink on existing symlink");
                }
            });

            Assert.Equal(JournalState.RolledBack, txFileSystem.Journal.State);
            Assert.True(txFileSystem.Journal.IsRolledBack);

            fileMock.Verify(f => f.CreateSymbolicLink(It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
        }
    }
}

#endif