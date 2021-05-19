namespace EQXMedia.TxFileSystem.Tests.Operations.Directories
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Journaling;
    using System;
    using System.IO.Abstractions.TestingHelpers;
    using System.Transactions;
    using Xunit;

    public sealed class DeleteOperation_Should
    {
        [Fact]
        public void DeleteOperation_Delete_ReturnsExistsFalse_ParentExistsTrue()
        {
            TxFileSystem txFileSystem = null;

            void CreateAndDeleteDirectory()
            {
                #region CodeExample_DeleteDirectory

                var mockFileSystem = new MockFileSystem();
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.CreateDirectory("/var/tobedeleted");
                    txFileSystem.Directory.Delete("/var/tobedeleted");
                    transactionScope.Complete();
                }

                #endregion
            }

            CreateAndDeleteDirectory();

            var txJournal = txFileSystem.Journal;

            Assert.Equal(JournalState.Committed, txJournal.State);
            Assert.False(txJournal.IsRolledBack);
            Assert.False(txFileSystem.Directory.Exists("/var/tobedeleted"));
            Assert.True(txFileSystem.Directory.Exists("/var"));
        }

        [Fact]
        public void DeleteOperation_Recursive_Delete_ReturnsExistsFalse_ParentExistsFalse()
        {
            TxFileSystem txFileSystem = null;

            void CreateAndDeleteDirectories()
            {
                #region CodeExample_DeleteDirectory_Recursive

                var mockFileSystem = new MockFileSystem();
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.CreateDirectory("/var/tobedeleted");
                    txFileSystem.Directory.Delete("/var", recursive: true);
                    transactionScope.Complete();
                }

                #endregion
            }

            CreateAndDeleteDirectories();

            var txJournal = txFileSystem.Journal;

            Assert.Equal(JournalState.Committed, txJournal.State);
            Assert.False(txJournal.IsRolledBack);
            Assert.False(txFileSystem.Directory.Exists("/var/tobedeleted"));
            Assert.False(txFileSystem.Directory.Exists("/var"));
        }

        [Fact]
        public void DeleteOperation_Fails_ResultsInExists_ReturnsTrue()
        {
            var mockFileSystem = new MockFileSystem();

            var txFileSystem = new TxFileSystem(mockFileSystem);
            txFileSystem.Directory.CreateDirectory("/var/failingdirectory");

            Assert.True(txFileSystem.Directory.Exists("/var/failingdirectory"));

            Assert.ThrowsAsync<Exception>(() =>
            {
                using (var transactionScope = new TransactionScope())
                {
                    txFileSystem = new TxFileSystem(mockFileSystem);
                    txFileSystem.Directory.Delete("/var/failingdirectory");

                    throw new Exception("Error while deleting failing directory");
                }
            });

            Assert.True(txFileSystem.Journal.IsRolledBack);
            Assert.True(txFileSystem.Directory.Exists("/var/failingdirectory"));
        }
    }
}
