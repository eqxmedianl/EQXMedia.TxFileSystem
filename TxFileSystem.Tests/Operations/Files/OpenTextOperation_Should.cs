namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class OpenTextOperation_Should
    {
        [Fact]
        public void OpenTextOperation_RetunsStreamReader()
        {
            var fileName = "/tmp/filetoopenastext.txt";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            var streamReader = txFileSystem.File.OpenText(fileName);

            transactionScope.Complete();

            Assert.IsAssignableFrom<StreamReader>(streamReader);
        }

        [Fact]
        public void OpenTextOperation_NotAddedToJournal()
        {
            var fileName = "/tmp/filetoopenastext.txt";

            var mockFileSystem = new MockFileSystem();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.CreateText(fileName);

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.OpenText(fileName);

            Assert.Empty(txFileSystem.Journal._txJournalEntries);
        }
    }
}
