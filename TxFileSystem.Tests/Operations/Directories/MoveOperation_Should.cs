namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class MoveOperation_Should
    {
        [Fact]
        public void MoveOperation_NoException_ResultsDirectoryMoved()
        {
            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/srv/www");
            txFileSystem.Directory.CreateDirectory("/var");
            txFileSystem.Directory.Move("/srv/www", "/var/www");
            transactionScope.Complete();

            Assert.False(txFileSystem.Directory.Exists("/srv/www"));
            Assert.True(txFileSystem.Directory.Exists("/var/www"));
        }

        [Fact]
        public void MoveOperation_ThrowsException_ResultsDirectoryNotMoved()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var");
            txFileSystem.Directory.CreateDirectory("/srv/www");

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.Move("/srv/www", "/var/www");

                throw new Exception("Error occurred right after moving the directory");
            });

            Assert.True(txFileSystem.Directory.Exists("/srv/www"));
            Assert.False(txFileSystem.Directory.Exists("/var/www"));
        }
    }
}
