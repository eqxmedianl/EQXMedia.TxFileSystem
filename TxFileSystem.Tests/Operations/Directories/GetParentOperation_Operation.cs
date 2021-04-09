namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class GetParentOperation_Operation
    {
        [Fact]
        public void GetParentOperation_ReturnsDirectoryInfo()
        {
            var dirName = "/tmp/dirtogetparentof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            var dirInfo = txFileSystem.Directory.GetParent(dirName);

            Assert.IsType<MockDirectoryInfo>(dirInfo);
        }

        [Fact]
        public void GetParentOperation_ReturnsCorrectName()
        {
            var dirName = "/tmp/dirtogetparentof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            var dirInfo = txFileSystem.Directory.GetParent(dirName);

            Assert.Equal("tmp", dirInfo.Name);
        }

        [Fact]
        public void GetParentOperation_NotAddedToJournal()
        {
            var dirName = "/tmp/dirtogetparentof";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory(dirName);

            using (var transactionScope = new TransactionScope())
            {
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.GetParent(dirName);
            }

            Assert.Empty(txFileSystem.Journal._txJournalEntries);
        }
    }
}
