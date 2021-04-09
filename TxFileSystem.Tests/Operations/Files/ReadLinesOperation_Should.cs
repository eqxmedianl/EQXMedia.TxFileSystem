namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Tests.Attributes;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Reflection;
    using System.Text;
    using System.Transactions;
    using Xunit;

    public sealed class ReadLinesOperation_Should
    {
        [Fact]
        public void ReadLinesOperation_ReturnsLinesWritten()
        {
            var fileName = "/tmp/filetoreadlinesfrom.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);
            txFileSystem.File.WriteAllLines(fileName, lines);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                var linesReturned = txFileSystem.File.ReadLines(fileName);

                transactionScope.Complete();

                Assert.Equal(lines, linesReturned);
            }
        }

        [Fact]
        public void ReadLinesOperation_NotAddedToJournal()
        {
            var fileName = "/tmp/filetoreadlinesfrom.txt";
            var lines = new string[] { "Line one", "Line two", "Line three" };

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);
            txFileSystem.File.WriteAllLines(fileName, lines);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.ReadLines(fileName);

                Assert.Empty(txFileSystem.Journal._txJournalEntries);
            }
        }

        [Fact, FsFact]
        public void ReadLinesOperation_CalledOnce_ReturnsLines()
        {
            var attr = (FsFactAttribute)Attribute.GetCustomAttribute(MethodBase.GetCurrentMethod(),
                typeof(FsFactAttribute));
            var fileSystemMock = attr.GetMock<IFileSystem>();

            var fileName = "/tmp/filetoreadlinesfrom.txt";
            var encoding = Encoding.UTF8;
            var lines = new string[] { "Line one", "Line two", "Line three" };

            fileSystemMock.Setup(fs => fs.File.ReadLines(It.Is<string>((s) => s == fileName),
                It.Is<Encoding>((e) => e == encoding)))
                .Returns(lines);

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(fileSystemMock.Object);
                var linesReturned = txFileSystem.File.ReadLines(fileName, encoding);

                transactionScope.Complete();

                fileSystemMock.Verify(fs => fs.File.ReadLines(It.Is<string>((s) => s == fileName),
                    It.Is<Encoding>((e) => e == encoding)), Times.Once);

                Assert.IsAssignableFrom<IEnumerable<string>>(linesReturned);
                Assert.Equal(lines, linesReturned);
            }
        }
    }
}
