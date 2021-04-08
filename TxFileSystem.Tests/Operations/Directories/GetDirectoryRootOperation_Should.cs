namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
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

    public sealed class GetDirectoryRootOperation_Should
    {
        [Fact]
        public void GetDirectoryRootOperation_NotAddedToJournal()
        {
            var dirName = "/tmp/dirtogetrootof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.Directory.CreateDirectory(dirName);

            using var transactionScope = new TransactionScope();
            txFileSystem = new TxFileSystem(mockFileSystem);

            txFileSystem.Directory.GetDirectoryRoot(dirName);

            Assert.Empty(txFileSystem.Journal._txJournalEntries);
        }

        [Fact, FsFact]
        public void GetDirectoryRootOperation_CalledOnce_SameRootDirectoryReturned()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();
            var txFileSystem = new TxFileSystem(fileSystemMock.Object);

            var dirName = "/tmp/dirtogetrootof";
            var rootDirectory = "/";

            fileSystemMock.Setup(f => f.Directory.GetDirectoryRoot(It.Is<string>((d) => d == dirName)))
                .Returns(rootDirectory);

            var rootDirectoryReturned = txFileSystem.Directory.GetDirectoryRoot(dirName);

            fileSystemMock.Verify(f => f.Directory.GetDirectoryRoot(It.Is<string>((d) => d == dirName)), Times.Once);

            Assert.Equal(rootDirectory, rootDirectoryReturned);
        }
    }
}
