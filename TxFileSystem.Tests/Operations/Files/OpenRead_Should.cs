namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.IO;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Transactions;
    using Xunit;

    public sealed class OpenRead_Should
    {
        [Fact]
        public void OpenRead_ReturnsStream()
        {
            var fileName = "/tmp/filetoopen.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            var fileStream = txFileSystem.File.OpenRead(fileName);

            Assert.IsAssignableFrom<Stream>(fileStream);
        }

        [Fact]
        public void OpenRead_NotAddedToJournal()
        {
            var fileName = "/tmp/filetoopen.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.OpenRead(fileName);

            var txJournal = txFileSystem.Journal;

            Assert.Empty(txJournal._txJournalEntries);
        }

        [Fact, FsFact]
        public void OpenRead_CalledOnce()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetoopenasreadonlystream.txt";

            fileSystemMock.Setup(f => f.File.OpenRead(It.Is<string>((s) => s == fileName)));

            var txFileSystem = new TxFileSystem(fileSystemMock.Object);
            txFileSystem.File.OpenRead(fileName);

            fileSystemMock.Verify(f => f.File.OpenRead(It.Is<string>((s) => s == fileName)), Times.Once);
        }
    }
}
