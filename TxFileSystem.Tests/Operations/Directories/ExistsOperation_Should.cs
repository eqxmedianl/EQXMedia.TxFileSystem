namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class ExistsOperation_Should
    {
        [Fact]
        public void ExistsOperation_ReturnsFalse()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);

            Assert.False(txFileSystem.Directory.Exists("/var/nonexistant"));
        }

        [Fact]
        public void ExistsOperation_ReturnsTrue()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/existant");

            Assert.True(txFileSystem.Directory.Exists("/var/existant"));
        }

        [Fact]
        public void ExistsOperation_ThrowsException_ReturnsFalse()
        {
            var mockFileSystem = new MockFileSystem();

            TxFileSystem txFileSystem = null;

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();
                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.Directory.CreateDirectory("/var/failafterthisdirectory");

                throw new Exception("Error occured right after creating directory");
            });

            Assert.True(((ITxFileSystem)txFileSystem).Journal.IsRolledBack);
            Assert.False(txFileSystem.Directory.Exists("/var/failafterthisdirectory"));
        }
    }
}
