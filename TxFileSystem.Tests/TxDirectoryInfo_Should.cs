namespace EQXMedia.TxFileSystem.Tests
{
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class TxDirectoryInfo_Should
    {
        [Fact]
        public void TxDirectoryInfo_ReturnsDirectoryInfo()
        {
            var dirName = "/tmp/dirtogetdirinfoof";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.Directory.CreateDirectory(dirName);
            var dirInfo = txFileSystem.DirectoryInfo.FromDirectoryName(dirName);

            transactionScope.Complete();

            Assert.IsType<MockDirectoryInfo>(dirInfo);
        }
    }
}
