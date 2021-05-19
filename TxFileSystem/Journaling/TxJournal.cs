namespace EQXMedia.TxFileSystem.Journaling
{
    using global::EQXMedia.TxFileSystem.Abstractions;
    using global::EQXMedia.TxFileSystem.Operations;
    using System.Linq;
    using System.Transactions;

    internal sealed class TxJournal : IEnlistmentNotification
    {
        internal readonly TxJournalEntryCollection _txJournalEntries;

        public TxJournal()
            : base()
        {
            _txJournalEntries = new TxJournalEntryCollection();

            if (Transaction.Current != null)
            {
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.EnlistDuringPrepareRequired);

                this.EnlistedInsideTransaction = true;
            }
        }

        internal bool EnlistedInsideTransaction { get; } = false;

        internal bool IsRolledBack { get { return (this.State == JournalState.RolledBack); } }

        internal JournalState State { get; private set; } = JournalState.Initial;

        internal void Add(IOperation operation)
        {
            if (this.State != JournalState.Initial)
            {
                return;
            }

            if (!_txJournalEntries.Any(e => e.Operation.Equals(operation)) && this.EnlistedInsideTransaction &&
                OperationRollbackGuard.ShouldRollback(operation.OperationType, this.State))
            {
                _txJournalEntries.Add(new TxJournalEntry(operation));
            }
        }

        public void Commit(Enlistment enlistment)
        {
            this.State = JournalState.Committing;
            foreach (var journalEntry in _txJournalEntries)
            {
                ((IEnlistmentOperation)journalEntry.Operation).Commit();
            }

            this.State = JournalState.Committed;
        }

        public void InDoubt(Enlistment enlistment)
        {
            Rollback(JournalState.InDoubt);
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            this.State = JournalState.Preparing;

            preparingEnlistment.Prepared();
        }

        public void Rollback(Enlistment enlistment)
        {
            Rollback(JournalState.RollingBack);
        }

        internal void Rollback(JournalState journalState)
        {
            this.State = journalState;
            foreach (var journalEntry in _txJournalEntries.Reverse())
            {
                ((IEnlistmentOperation)journalEntry.Operation).Rollback();
            }
            _txJournalEntries.Clear();

            this.State = JournalState.RolledBack;
        }
    }
}
