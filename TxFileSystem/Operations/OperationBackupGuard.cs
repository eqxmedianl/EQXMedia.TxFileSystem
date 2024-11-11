namespace EQXMedia.TxFileSystem.Operations
{
    using global::EQXMedia.TxFileSystem.Journaling;
    using System.Collections.Generic;

    internal static class OperationBackupGuard
    {
        public static IList<OperationType> NonBackupableOperationTypes => new List<OperationType>()
        {
            OperationType.Create,
            OperationType.CreateSymlink,
            OperationType.Info,
            OperationType.Navigate,
            OperationType.Read
        };

        public static bool ShouldBackup(TxJournal txJournal, OperationType operationType)
        {
            return (!NonBackupableOperationTypes.Contains(operationType) &&
                txJournal.EnlistedInsideTransaction && txJournal.State == JournalState.Initial);
        }

        public static bool ShouldRestore(TxJournal txJournal, OperationType operationType)
        {
            return (!NonBackupableOperationTypes.Contains(operationType) &&
                txJournal.EnlistedInsideTransaction && txJournal.State == JournalState.RollingBack);
        }
    }
}
