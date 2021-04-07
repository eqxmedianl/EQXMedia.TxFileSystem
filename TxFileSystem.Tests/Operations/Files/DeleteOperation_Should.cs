namespace EQXMedia.TxFileSystem.Tests.Operations.Files
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class DeleteOperation_Should
    {
        [Fact]
        public void DeleteOperation_File_DoesntExistAfterDeletion()
        {
            var fileName = "/tmp/filetobedeleted.txt";

            var mockFileSystem = new MockFileSystem();

            using var transactionScope = new TransactionScope();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);
            txFileSystem.File.Delete(fileName);

            transactionScope.Complete();

            Assert.False(txFileSystem.File.Exists(fileName));
        }

        [Fact]
        public void DeleteOperation_File_AddedToJournal()
        {
            var fileName = "/tmp/filetobedeleted.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            using var transactionScope = new TransactionScope();

            txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.File.Delete(fileName);

            Assert.NotEmpty(((ITxFileSystem)txFileSystem).Journal._txJournalEntries);
        }

        [Fact]
        public void DeleteOperation_ExceptionThrown_FileExists()
        {
            var fileName = "/tmp/filetobedeleted.txt";

            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/tmp");
            txFileSystem.File.Create(fileName);

            Assert.ThrowsAsync<Exception>(() =>
            {
                using var transactionScope = new TransactionScope();

                txFileSystem = new TxFileSystem(mockFileSystem);
                txFileSystem.File.Delete(fileName);

                throw new Exception("Error right after deleting file");
            });

            Assert.True(txFileSystem.File.Exists(fileName));
        }
    }
}
