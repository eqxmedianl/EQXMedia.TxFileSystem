namespace EQXMedia.TxFileSystem.Operations
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using System.Collections.Generic;

    internal static class OperationRollbackGuard
    {
        public static bool ShouldRollback(OperationType operationType, JournalState journalState)
        {
            var nonRollbackableOperations = new List<OperationType>()
            {
                OperationType.Info,
                OperationType.Read
            };

            var nonRollbackableJournalStates = new List<JournalState>()
            {
                JournalState.Committed,
                JournalState.Committing,
                JournalState.RolledBack
            };

            return (!nonRollbackableOperations.Contains(operationType)
                && !nonRollbackableJournalStates.Contains(journalState));
        }
    }
}
