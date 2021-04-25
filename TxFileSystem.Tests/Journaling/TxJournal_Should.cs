namespace EQXMedia.TxFileSystem.Tests.Journaling
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using global::EQXMedia.TxFileSystem.Operations;
    using global::EQXMedia.TxFileSystem.Tests.Operations;
#if SUPPRESS_SIMPLE_USING
    using System.Diagnostics.CodeAnalysis;
#endif
    using System.IO.Abstractions.TestingHelpers;
    using System.Linq;
    using System.Transactions;
    using Xunit;

    public sealed class TxJournal_Should
    {
        [Fact]
#if SUPPRESS_SIMPLE_USING
        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement",
            Justification = "This library is supporting framework versions relying on older language versions")]
#endif
        public void TxJournal_Add_Results_JournalEntriesCountsOne()
        {
            var mockFileSystem = new MockFileSystem();

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(mockFileSystem);
                var txJournal = txFileSystem.Journal;
                txJournal.Add(new UnitTestDirectoryOperation(txFileSystem.Directory,
                    "/var/journaltestdir"));

                Assert.Single(txFileSystem.Journal._txJournalEntries);
            }
        }

        [Fact]
        public void TxJournal_Add_Results_FirstEntryOperationType_ReturnsInfo()
        {
            var mockFileSystem = new MockFileSystem();

            TxJournalEntry firstJournalEntry = null;

            using (var transactionScope = new TransactionScope())
            {
                var txFileSystem = new TxFileSystem(mockFileSystem);
                var txJournal = txFileSystem.Journal;
                txJournal.Add(new UnitTestDirectoryOperation(txFileSystem.Directory,
                    "/var/journaltestdir"));

                firstJournalEntry = txJournal._txJournalEntries.First();
            }

            Assert.Equal(OperationType.Delete, firstJournalEntry.Operation.OperationType);
        }

        [Fact]
        public void TxJournal_IsRolledBack_ReturnsFalse()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var txJournal = txFileSystem.Journal;

            Assert.False(txJournal.IsRolledBack);
        }

        [Fact]
        public void TxJournal_Rollback_IsRolledBack_ReturnsTrue()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var txJournal = txFileSystem.Journal;
            txJournal.Rollback(JournalState.RollingBack);

            Assert.True(txJournal.IsRolledBack);
        }

        [Fact]
        public void TxJournal_InDoubt_State_ReturnsInDoubt()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var txJournal = txFileSystem.Journal;
            using (var transactionScope = new TransactionScope())
            {
                var enlistment = Transaction.Current.EnlistVolatile(txJournal,
                    EnlistmentOptions.EnlistDuringPrepareRequired);
                txJournal.InDoubt(enlistment);
            }

            Assert.Equal(JournalState.RolledBack, txJournal.State);
            Assert.True(txJournal.IsRolledBack);
        }

        [Fact]
        public void TxJournal_Initial_State_ReturnsInitial()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var txJournal = txFileSystem.Journal;

            Assert.Equal(JournalState.Initial, txJournal.State);
            Assert.False(txJournal.IsRolledBack);
        }

        [Fact]
        public void TxJournal_Rollback_State_ReturnsRollback()
        {
            var mockFileSystem = new MockFileSystem();
            var txFileSystem = new TxFileSystem(mockFileSystem);
            var txJournal = txFileSystem.Journal;
            using (var transactionScope = new TransactionScope())
            {
                var enlistment = Transaction.Current.EnlistVolatile(txJournal,
                    EnlistmentOptions.EnlistDuringPrepareRequired);

                txJournal.Rollback(enlistment);
            }

            Assert.Equal(JournalState.RolledBack, txJournal.State);
            Assert.True(txJournal.IsRolledBack);
        }
    }
}
