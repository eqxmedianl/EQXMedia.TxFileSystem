namespace EQXMedia.TxFileSystem.Tests
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class TxFileInfo_Should
    {
        [Fact]
        public void TxFileInfo_ReturnsFileInfo()
        {
            var mockFileSystem = new MockFileSystem();

            var fileName = "/tmp/filetogetinfoof.txt";

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);
            var fileInfo = txFileSystem.FileInfo.FromFileName(fileName);

            transactionScope.Complete();

            Assert.IsType<MockFileInfo>(fileInfo);
        }

        [Fact, FsFact]
        public void TxFileInfo_FromFileName_CalledOnce_ReturnsFileInfo()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetogetinfoof.txt";
            var baseName = "filetogetinfoof.txt";

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.SetupGet(d => d.Name)
                .Returns(baseName);

            fileSystemMock.Setup(f => f.FileInfo.FromFileName(It.Is<string>((s) => s == fileName)))
                .Returns(fileInfoMock.Object);

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            var fileInfoReturned = txFileSystem.FileInfo.FromFileName(fileName);

            fileSystemMock.Verify(f => f.FileInfo.FromFileName(It.Is<string>((s) => s == fileName)),
                Times.Once);

            Assert.IsAssignableFrom<IFileInfo>(fileInfoReturned);
            Assert.Equal(baseName, fileInfoReturned.Name);
        }

        [Fact]
        public void TxFileInfo_NotAddedToJournal()
        {
            var fileName = "/tmp/filetogetinfoof.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            var fileInfo = txFileSystem.FileInfo.FromFileName(fileName);

            transactionScope.Complete();

            Assert.IsType<MockFileInfo>(fileInfo);

            Assert.Empty(((ITxFileSystem)txFileSystem).Journal._txJournalEntries);
        }
    }
}
